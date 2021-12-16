using System;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Controllers.Responses;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Utils.Jwt.JwtUtils;

namespace HousingAssociation.Services
{
    public class AuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtUtils _jwtUtils;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(IUnitOfWork unitOfWork, IJwtUtils jwtUtils, JwtConfig jwtConfig)
        {
            _unitOfWork = unitOfWork;
            _jwtUtils = jwtUtils;
            _jwtConfig = jwtConfig;
        }
        
        public async Task<User> RegisterUser(RegisterRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = Role.Resident,
                IsEnabled = false
            };
            
            user = await _unitOfWork.UsersRepository.AddIfNotExists(user) ?? throw new BadRequestException("User already exists!");

            var credentials = new UserCredentials
            {
                User = user,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            await _unitOfWork.CommitAsync();
            
            return user;
        }
        
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _unitOfWork.UsersRepository.FindByUserEmailAsync(request.Email) ??
                       throw new BadRequestException("Email or password incorrect");

            var passwordHash = (await _unitOfWork.UserCredentialsRepository.FindByUserId(user.Id)).PasswordHash;

            // validate
            if (!BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
                throw new BadRequestException("Email or password incorrect");

            if (!user.IsEnabled)
                throw new BadRequestException("User is not enabled");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return new LoginResponse(user, jwtToken, refreshToken.Token);
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var user = await _unitOfWork.UsersRepository.FindByUserEmailAsync(request.Email);
            if (user is null || !user.PhoneNumber.Equals(request.PhoneNumber))
                throw new NotFoundException();
            user.UserCredentials.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _unitOfWork.CommitAsync();
        }
        
        public async Task<LoginResponse> RefreshToken(string token)
        {
            var user = await _unitOfWork.UsersRepository.FindByRefreshTokenAsync(token) ??
                       throw new BadRequestException("Invalid token");
            
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, user, $"Attempted reuse of revoked ancestor token: {token}");
                _unitOfWork.UsersRepository.Update(user);
                await _unitOfWork.CommitAsync();
            }

            if (!refreshToken.IsActive)
                throw new BadRequestException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = RotateRefreshToken(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.CommitAsync();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new LoginResponse(user, jwtToken, newRefreshToken.Token);
        }
        
        public async Task RevokeToken(string token)
        {
            var user = await _unitOfWork.UsersRepository.FindByRefreshTokenAsync(token) ??
                       throw new BadRequestException("Invalid token");
            
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new BadRequestException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, "Revoked without replacement");
            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.CommitAsync();
        }
        
        private void RemoveOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(token => 
                !token.IsActive && 
                token.Created.AddDays(_jwtConfig.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken();
            RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, reason);
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

    }
}