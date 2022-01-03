using System;
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
using Serilog;

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
            {
                Log.Warning($"User with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            
            user.IsEnabled = true;
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

            user = await _unitOfWork.UsersRepository.AddIfNotExists(user);
            if (user.Id != 0)
            {
                Log.Warning($"User with id = {user.Id} already exists.");
                throw new BadRequestException("User already exists");
            }
            
            var password = PasswordGenerator.CreateRandomPassword();

            var credentials = new UserCredentials
            {
                User = user,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            
            await _unitOfWork.UserCredentialsRepository.Add(credentials);
            try
            {
                _emailService.SendEmail(PrepareMessageWithPassword(user, password));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception exception)
            {
                Log.Error($"Problem while attempt to send email to user {user.Id}.");
                Log.Error(exception, $"{ exception.Message } at { exception.Source }.");
            }
            return user.AsDto();
        }
        
        public async Task Update(UserDto userDto)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(userDto.Id);
            if (user is null)
            {
                Log.Warning($"User with id = {userDto.Id} doesn't exist.");
                throw new NotFoundException();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Role = userDto.Role ?? user.Role;
            user.IsEnabled = userDto.IsEnabled ?? user.IsEnabled;
        
            await _unitOfWork.CommitAsync();
        }

        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var credentials = await _unitOfWork.UserCredentialsRepository.FindByUserId(userId);
            if (credentials is null)
            {
                Log.Error($"User with id = {userId} doesn't exist.");
                throw new NotFoundException();
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, credentials.PasswordHash))
            {
                Log.Warning($"User with id = {userId} didn't match old password.");
                throw new BadRequestException("Old password not valid");
            }

            credentials.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _unitOfWork.CommitAsync();
        }
        
        public async Task DisableUser(int id, string password = null, bool removeCredentials = false)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(id);
            if (user is null)
            {
                Log.Warning($"User with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            var credentials = await _unitOfWork.UserCredentialsRepository.FindByUserId(user.Id);
            if (credentials is null)
            {
                Log.Warning($"Trying to disable user (id = {user.Id}) with no credentials.");
                throw new NotFoundException();
            }
            if (password is not null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, credentials.PasswordHash))
                {
                    Log.Warning($"Attempt to disable user with id = {id} with incorrect password given.");
                    throw new BadRequestException();
                }
            }
            if (removeCredentials)
            {
                _unitOfWork.UserCredentialsRepository.Delete(credentials);
            }
            user.IsEnabled = false;
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.FindByIdAsync(id);
            if (user is null)
            {
                Log.Warning($"User with id = {id} doesn't exist.");
                throw new BadRequestException($"User with id {id} doesn't exist");
            }
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