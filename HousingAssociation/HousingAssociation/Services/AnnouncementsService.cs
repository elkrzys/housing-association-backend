using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;

namespace HousingAssociation.Services
{
    public class AnnouncementsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnnouncementsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAnnouncementByBuildingsIds(AnnouncementDto announcementDto)
        {
            if (announcementDto is null)
                throw new BadRequestException("Announcement incorrect");

            List<Building> buildings = new();
            
            announcementDto.TargetBuildingsIds.ForEach(async bId =>
            {
                var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(bId);
                if(building is not null)
                    buildings.Add(building);
            });

            if (!buildings.Any())
                throw new BadRequestException("Announcement must have target buildings");

            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }
        
        public async Task AddAnnouncementByAddress(AnnouncementDto announcementDto)
        {
            if (!announcementDto.Addresses.Any())
                throw new BadRequestException("No target address defined");

            List<Building> buildings = new();
            announcementDto.Addresses.ForEach(async address =>
            {
                var buildingsFromAddress = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
                buildings = buildings.Concat(buildingsFromAddress).Distinct().ToList();
            });
            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }

        public async Task UpdateAnnouncement(AnnouncementDto announcementDto)
        {
            if (announcementDto.Id is null)
                throw new BadRequestException("Announcement incorrect");

            await CancelAnnouncementById(announcementDto.Id!.Value);

            List<Building> buildings = new();

            if (announcementDto.TargetBuildingsIds.Any())
            {
                announcementDto.TargetBuildingsIds.ForEach(async bId =>
                {
                    var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(bId);
                    if(building is not null)
                        buildings.Add(building);
                });
            }
            else if (announcementDto.Addresses.Any())
            {
                announcementDto.Addresses.ForEach(async address =>
                {
                    var buildingsFromAddress = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
                    buildings = buildings.Concat(buildingsFromAddress).Distinct().ToList();
                });
            }
            else
            {
                buildings = await _unitOfWork.BuildingsRepository.FindAllAsync();
            }

            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }

        public async Task DeleteAnnouncement(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindById(id) ?? throw new NotFoundException();
            _unitOfWork.AnnouncementsRepository.Delete(announcement);
            _unitOfWork.Commit();
        }

        public async Task<List<Announcement>> GetAll() => await _unitOfWork.AnnouncementsRepository.FindAll();

        public async Task<List<Announcement>> GetAllByBuildingId(int buildingId) =>
            await _unitOfWork.AnnouncementsRepository.FindAllByTargetBuildingId(buildingId);
        
        public async Task<List<Announcement>> GetAllByAddress(Address address)
        {
            if (address is null) 
                throw new BadRequestException("Address must not be null");

            return await _unitOfWork.AnnouncementsRepository.FindAllByAddress(address);
        }

        public async Task<List<AnnouncementDto>> GetAllByReceiverId(int receiverId)
        {
            var receiver = await _unitOfWork.UsersRepository.FindByIdAndIncludeAllLocals(receiverId) 
                           ?? throw new NotFoundException();

            var receiverLocals = receiver.ResidedLocals
                .Concat(receiver.OwnedLocals)
                .Distinct()
                .ToList();
            
            List<Announcement> announcements = new();

            receiverLocals.ForEach(async local =>
            {
                var b = await _unitOfWork.BuildingsRepository.FindByIdAsync(local.BuildingId);
                if (b is not null)
                {
                    var buildingAnnouncements =
                        await _unitOfWork.AnnouncementsRepository.FindAllByTargetBuildingId(local.BuildingId);
                    announcements = announcements.Concat(buildingAnnouncements).Distinct().ToList();
                }
            });

            return GetAnnouncementsAsDtos(announcements);
        }

        public async Task<List<AnnouncementDto>> GetAllByAuthorId(int authorId)
        {
            var announcements =  await _unitOfWork.AnnouncementsRepository.FindAllByAuthorId(authorId);
            return (announcements is not null) ? GetAnnouncementsAsDtos(announcements) : null;
        }

        public async Task CancelAnnouncementById(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindById(id) 
                               ?? throw new NotFoundException();

            announcement.IsCancelledOrExpired = true;
            
            await _unitOfWork.AnnouncementsRepository.Update(announcement);
            _unitOfWork.Commit();
        }

        public async Task<List<AnnouncementDto>> GetAllFiltered(AnnouncementsFilterRequest filter)
        {
            if (filter is null)
                throw new BadRequestException();
            filter.Address ??= new Address();
            var announcements =  await _unitOfWork.AnnouncementsRepository.FindAllFiltered(filter);
            return GetAnnouncementsAsDtos(announcements);
        }

        private async Task AddAnnouncementWithBuildings(AnnouncementDto announcementDto, List<Building> buildings)
        {
            var announcement = new Announcement
            {
                TargetBuildings = buildings,
                AuthorId = announcementDto.AuthorId,
                CreatedAt = DateTime.Now,
                Content = announcementDto.Content,
                Title = announcementDto.Title,
                ExpirationDate = announcementDto.ExpirationDate,
                Type = announcementDto.Type
            };

            await _unitOfWork.AnnouncementsRepository.Add(announcement);
            _unitOfWork.Commit();
        }

        private List<AnnouncementDto> GetAnnouncementsAsDtos(List<Announcement> announcements)
        {
            List<AnnouncementDto> announcementDtos = new();
            announcements.ForEach(a => announcementDtos.Add(a.AsDto()));
            return announcementDtos;
        }
        
    }
}