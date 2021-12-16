using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils;
using HousingAssociation.Utils.Extensions;
using Microsoft.OpenApi.Expressions;
using MimeKit;

namespace HousingAssociation.Services
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;

        public UsersService(IUnitOfWork unitOfWork, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<List<UserDto>> FindUnconfirmedUsers()
        {
            var users = await _unitOfWork.UsersRepository.FindAllNotEnabledUsersAsync();
            return GetUsersAsDtos(users);
        }
        
        public async Task<List<UserDto>> FindAllResidents()
        {
            var users = await _unitOfWork.UsersRepository.FindByRoleAsync(Role.Resident);
            return GetUsersAsDtos(users);
        }
        
        public async Task<List<UserDto>> FindAllWorkers()
        {
            var users = await _unitOfWork.UsersRepository.FindByRoleAsync(Role.Worker);
            return GetUsersAsDtos(users);
        }
        
        public async Task<UserDto> FindUserById(int id) => (await _unitOfWork.UsersRepository.FindByIdAsync(id)).AsDto();
        public async Task<UserDto> ConfirmUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(id);
            if (user is null)
                throw new BadRequestException();
            
            _unitOfWork.UsersRepository.Update(user with {IsEnabled = true});
            await _unitOfWork.CommitAsync();
            
            return user.AsDto();
        }

        public async Task<UserDto> AddWorker(UserDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email,
                Role = Role.Worker,
                IsEnabled = true
            };
            
            user = await _unitOfWork.UsersRepository.AddIfNotExists(user) ??
                   throw new BadRequestException("User already exists");
            
            var password = PasswordGenerator.CreateRandomPassword();

            var credentials = new UserCredentials
            {
                User = user,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            _emailService.SendEmail(PrepareMessageWithPassword(user, password));
            await _unitOfWork.CommitAsync();
            
            return user.AsDto();
        }
        
        public async Task Update(UserDto userDto)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(userDto.Id);
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
            await _unitOfWork.CommitAsync();
        }

        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var credentials = await _unitOfWork.UserCredentialsRepository.FindByUserId(userId);
            if (credentials is null)
                throw new BadRequestException("User doesn't exist");
                    
            if(!BCrypt.Net.BCrypt.Verify(request.OldPassword, credentials.PasswordHash))
                throw new BadRequestException("Old password not valid");
            
            _unitOfWork.UserCredentialsRepository.Update(credentials with {PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword)});
            await _unitOfWork.CommitAsync();
        }
        public async Task DisableUser(UserDto userDto)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(userDto.Id) ?? throw new NotFoundException();
            
            _unitOfWork.UsersRepository.Update(user with {IsEnabled = false});
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(id);
            if (user is null)
                throw new BadRequestException($"User with id {id} doesn't exist");
            
            _unitOfWork.UsersRepository.Delete(user);
        }
        
        private List<UserDto> GetUsersAsDtos(List<User> users) => users.Select(user => user.AsDto()).ToList();

        private MailMessage PrepareMessageWithPassword(User receiver, string password)
        {
            var from = new MailboxAddress("Wspólnota mieszkaniowa", _emailService.GetAppMailAddress);
            return new MailMessage(from, new[] { MailboxAddress.Parse(receiver.Email) }, true)
            {
                Subject = "Rejestracja konta",
                Message = $"<h1>Witaj {receiver.FirstName} {receiver.LastName}.</h1><br/>" +
                          "Zostało dla ciebie utworzone konto pracownicze.<br/>" +
                          $"<p>Aby się zalogować użyj tego hasła: <b>{password}</b></p>" +
                          $"<p style=\"color:red\">Pamiętaj aby zmienić hasło po zalogowaniu!</p>"
            };
        }
        
    }
}