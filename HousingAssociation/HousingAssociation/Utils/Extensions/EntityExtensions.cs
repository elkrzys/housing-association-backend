using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HousingAssociation.Utils.Extensions
{
    public static class EntityExtensions
    {
        public static UserDto AsDto(this User user) 
            => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsEnabled = user.IsEnabled
            };
        
    }
}