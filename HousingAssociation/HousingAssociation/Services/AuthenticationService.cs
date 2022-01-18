using System;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Controllers.Responses;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Utils.Jwt.JwtUtils;
using Serilog;

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
        
        public async Task RegisterUser(RegisterRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = Role.Resident,
                IsEnabled = true
            };
            var credentials = new UserCredentials
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            var existingUser = await GetUserOrNullIfRegistrationPossible(user);
            if (existingUser is not null)
            {
                existingUser.IsEnabled = true;
                credentials.User = existingUser;
            }
            else
            {
                await _unitOfWork.UsersRepository.AddAsync(user);
                credentials.User = user;
            }
            
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            await _unitOfWork.CommitAsync();
        }

        private async Task<User> GetUserOrNullIfRegistrationPossible(User user)
        {
            var existingUser = await _unitOfWork.UsersRepository.FindByUserEmailAsync(user.Email);
            if (existingUser is null) return null;
            
            if (existingUser.IsEnabled)
            {
                Log.Warning($"Trying to register user with data of user with id = {existingUser.Id}");
                throw new BadRequestException("User already exists!");
            }
            if (existingUser.UserCredentials is not null)
            {
                Log.Warning($"Trying to register banned user. User id = {existingUser.Id}");
                throw new BadRequestException("User banned!");
            }
            return existingUser;
        }
        
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _unitOfWork.UsersRepository.FindByUserEmailAsync(request.Email) ??
                       throw new BadRequestException("Email or password incorrect");

            var passwordHash = (await _unitOfWork.UserCredentialsRepository.FindByUserId(user.Id)).PasswordHash;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
                throw new BadRequestException("Email or password incorrect");

            if (!user.IsEnabled)
                throw new BadRequestException("User is not enabled");

            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            RemoveOldRefreshTokens(user);

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
                RevokeDescendantRefreshTokens(refreshToken, user, $"Attempted reuse of revoked ancestor token: {token}");
                _unitOfWork.UsersRepository.Update(user);
                await _unitOfWork.CommitAsync();
            }

            if (!refreshToken.IsActive)
                throw new BadRequestException("Invalid token");

            var newRefreshToken = RotateRefreshToken(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            RemoveOldRefreshTokens(user);

            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.CommitAsync();

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
            
            RevokeRefreshToken(refreshToken, "Revoked without replacement");
            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.CommitAsync();
        }
        
        private void RemoveOldRefreshTokens(User user)
        {
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