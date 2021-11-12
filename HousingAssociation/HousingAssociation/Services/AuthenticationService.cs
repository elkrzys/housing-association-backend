using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Services
{
    public class AuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<User> RegisterUser(RegisterRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = Role.Resident,
                IsEnabled = false
            };
            
            user = await _unitOfWork.UsersRepository.Add(user);
            
            var credentials = new UserCredentials
            {
                User = user,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            _unitOfWork.Commit();

            return user;
        }
        
        public Task Login()
        {
            // generate token
            // generate refresh token
            // add refresh token to db
            // return jwt and set user as logged in
            return null;
        }
        
        public Task Logout()
        {
            // delete refresh token from db
            // logout user
            return null;
        }
    }
}