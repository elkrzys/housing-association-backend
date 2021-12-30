using System.Linq;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;

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
                Created = document.Created,
                Removes = document.Removes,
                FilePath = document.Filepath,
                Author = new Author
                {
                    Id = document.AuthorId, 
                    FirstName = document.Author.FirstName,
                    LastName = document.Author.LastName
                }
            };

        public static AnnouncementDto AsDto(this Announcement announcement)
            => new AnnouncementDto
            {
                Id = announcement.Id,
                Author = new Author
                {
                    Id = announcement.AuthorId,
                    FirstName = announcement.Author.FirstName,
                    LastName = announcement.Author.LastName
                },
                Content = announcement.Content,
                ExpirationDate = announcement.ExpirationDate,
                Title = announcement.Title,
                Type = announcement.Type,
                Created = announcement.Created,
                Addresses = announcement.TargetBuildings?.Select(building => building.Address.AsDto()).ToList(),
                TargetBuildingsIds = announcement.TargetBuildings?.Select(building => building.Id).ToList()
            };
        
        public static BuildingDto AsDto(this Building building)
            => new BuildingDto
            {
                Id = building.Id,
                Address = building.Address,
                Type = building.Type,
                Number = building.Number,
                Locals = building.Locals?.Select(local => local.AsDto()),
                NumberOfLocals = building.Locals?.Count
            };
        
        public static IssueDto AsDto(this Issue issue)
            => new IssueDto
            {
                Id = issue.Id,
                //AuthorId = issue.AuthorId,
                Title = issue.Title,
                Content = issue.Content,
                Created = issue.Created.ToString("yyyy-MM-ddTHH:mm:ss"),
                Cancelled = issue.Cancelled?.ToString("yyyy-MM-ddTHH:mm:ss"),
                Resolved = issue.Resolved?.ToString("yyyy-MM-ddTHH:mm:ss"),
                SourceBuildingId = issue.Local?.BuildingId,
                SourceLocalId = issue.SourceLocalId ?? 0,
                Address = issue.Local?.Building?.Address,
                Author = new Author
                {
                    Id = issue.AuthorId, 
                    FirstName = issue.Author?.FirstName, 
                    LastName = issue.Author?.LastName
                }
            };

        public static LocalDto AsDto(this Local local)
            => new LocalDto
            {
                Id = local.Id,
                BuildingId = local.BuildingId,
                BuildingNumber = local.Building?.Number,
                Area = local.Area,
                Number = local.Number,
                IsFullyOwned = local.IsFullyOwned,
                NumberOfResidents = local.Residents?.Count,
                Address = local.Building?.Address
            };

        public static AddressDto AsDto(this Address address)
            => new AddressDto
            {
                City = address.City,
                District = address.District,
                Street = address.Street
            };
    }
}