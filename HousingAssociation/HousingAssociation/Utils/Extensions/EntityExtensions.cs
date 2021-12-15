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
                Created = document.Created,
                Removes = document.Removes,
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
        
        public static BuildingDto AsDto(this Building building)
            => new BuildingDto
            {
                Id = building.Id,
                Address = building.Address,
                Type = building.Type,
                Number = building.Number,
                NumberOfLocals = building.Locals?.Count
            };
        
        public static IssueDto AsDto(this Issue issue)
            => new IssueDto
            {
                Id = issue.Id,
                AuthorId = issue.AuthorId,
                Title = issue.Title,
                Content = issue.Content,
                Cancelled = issue.Cancelled,
                Resolved = issue.Resolved,
                SourceBuildingId = issue.Local.BuildingId,
                SourceLocalId = issue.SourceLocalId ?? 0,
                Address = issue.Local?.Building?.Address
            };

        public static LocalDto AsDto(this Local local)
            => new LocalDto
            {
                Id = local.Id,
                BuildingId = local.BuildingId,
                Area = local.Area,
                Number = local.Number,
                IsFullyOwned = local.IsFullyOwned,
                NumberOfResidents = local.Residents?.Count
            };
    }
}