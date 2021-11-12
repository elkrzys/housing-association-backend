using System.Collections.Generic;
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

        public async Task<List<User>> FindUnconfirmedUsers() => await _unitOfWork.UsersRepository.FindAllNotEnabledUsers();
        public async Task<User> FindUserById(int id) => await _unitOfWork.UsersRepository.FindById(id);

        public async Task<User> ConfirmUser(User user)
        {
            await _unitOfWork.UsersRepository.Update(user with {IsEnabled = true});
            _unitOfWork.Commit();
            
            return user;
        }
        
        public Task Update()
        {
            return null;
        }
        
        public Task DeleteUser()
        {
            return null;
        }

        
    }
}