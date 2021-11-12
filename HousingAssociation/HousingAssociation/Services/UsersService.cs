using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Services
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            user = await _unitOfWork.UsersRepository.Add(user);
            
            var credentials = new UserCredentials
            {
                User = user,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            _unitOfWork.Commit();

            return user;
        }
        
        public Task ConfirmUser()
        {
            // update user with his new role and isEnabled
            return null;
        }
        
        public Task Update()
        {
            return null;
        }
        
        public Task DeleteUser()
        {
            return null;
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