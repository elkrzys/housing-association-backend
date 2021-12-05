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
        
        public static DocumentDto AsDto(this Document document) 
            => new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                AuthorId = document.AuthorId,
                CreatedAt = document.CreatedAt,
                RemovesAt = document.RemovesAt,
                FilePath = document.Filepath
            };

        public static AnnouncementDto AsDto(this Announcement announcement)
            => new AnnouncementDto
            {
                Id = announcement.Id,
                AuthorId = announcement.AuthorId,
                Content = announcement.Content,
                ExpirationDate = announcement.ExpirationDate,
                Title = announcement.Title,
                Type = announcement.Type
            };

    }
}