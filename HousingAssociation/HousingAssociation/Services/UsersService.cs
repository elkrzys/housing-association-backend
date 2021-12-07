using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Microsoft.OpenApi.Expressions;

namespace HousingAssociation.Services
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> FindUnconfirmedUsers()
        {
            var users = await _unitOfWork.UsersRepository.FindAllNotEnabledUsers();
            List<UserDto> usersDtos = new();
            users.ForEach(u => usersDtos.Add(u.AsDto()));
            return usersDtos;
        }
        public async Task<UserDto> FindUserById(int id) => (await _unitOfWork.UsersRepository.FindById(id)).AsDto();
        public async Task<UserDto> ConfirmUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.FindById(id);
            if (user is null)
                throw new BadRequestException();
            
            _unitOfWork.UsersRepository.Update(user with {IsEnabled = true});
            _unitOfWork.Commit();
            
            return user.AsDto();
        }

        public async Task AddWorker(RegisterRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = Role.Worker,
                IsEnabled = false
            };
            
            // TODO: generate worker credentials
            user = await _unitOfWork.UsersRepository.AddIfNotExists(user);
        }
        
        public async Task Update(UserDto userDto)
        {
            var user = await _unitOfWork.UsersRepository.FindById(userDto.Id);
            if (user is null)
                throw new BadRequestException($"User with id {userDto.Id} doesn't exist");
            
            _unitOfWork.UsersRepository.Update(user with
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = userDto.Role ?? user.Role,
                IsEnabled = userDto.IsEnabled ?? user.IsEnabled
            });
            _unitOfWork.Commit();
        }

        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var credentials = await _unitOfWork.UserCredentialsRepository.FindByUserId(userId);
            if (credentials is null)
                throw new BadRequestException("User doesn't exist");
                    
            if(!BCrypt.Net.BCrypt.Verify(request.OldPassword, credentials.PasswordHash))
                throw new BadRequestException("Old password not valid");
            
            _unitOfWork.UserCredentialsRepository.Update(credentials with {PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword)});
            _unitOfWork.Commit();
        }
        
        public async Task DeleteUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.FindById(id);
            if (user is null)
                throw new BadRequestException($"User with id {id} doesn't exist");
            
            _unitOfWork.UsersRepository.Delete(user);
        }

        
    }
}