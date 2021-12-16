using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
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

            foreach (var buildingId in announcementDto.TargetBuildingsIds)
            {
                var building = await _unitOfWork.BuildingsRepository.FindByIdWithAddressAsync(buildingId);
                if (building is not null)
                {
                    _unitOfWork.SetModified(building);
                    buildings.Add(building);
                }
            }

            if (!buildings.Any())
                throw new BadRequestException("Announcement must have target buildings");

            await AddAnnouncementWithBuildings(announcementDto, buildings);
            
        }
        
        public async Task AddAnnouncementByAddress(AnnouncementDto announcementDto)
        {
            if (!announcementDto.Addresses.Any())
                throw new BadRequestException("No target address defined");

            List<Building> buildings = new();
            foreach (var address in announcementDto.Addresses)
            {
                var buildingsFromAddress = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
                buildings = buildings.Concat(buildingsFromAddress).Distinct().ToList();
            }
            buildings.ForEach(b => _unitOfWork.SetModified(b));
            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }

        public async Task UpdateAnnouncement(AnnouncementDto announcementDto)
        {
            if (announcementDto.Id is null)
                throw new BadRequestException("Announcement incorrect");

            await CancelAnnouncementById(announcementDto.Id!.Value);

            if (announcementDto.TargetBuildingsIds.Any())
            {
                // foreach (var buildingId in announcementDto.TargetBuildingsIds)
                // {
                //     var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(buildingId);
                //     if(building is not null)
                //         buildings.Add(building);
                // }
                await AddAnnouncementByBuildingsIds(announcementDto);
            }
            else if (announcementDto.Addresses.Any())
            {
                // foreach (var address in announcementDto.Addresses)
                // {
                //     var buildingsFromAddress = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
                //     buildings = buildings.Concat(buildingsFromAddress).Distinct().ToList();
                // }
                await AddAnnouncementByAddress(announcementDto);
            }
            else
            {
                var buildings = await _unitOfWork.BuildingsRepository.FindAllAsync();
                await AddAnnouncementWithBuildings(announcementDto, buildings);
            }
        }

        public async Task DeleteAnnouncement(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            _unitOfWork.AnnouncementsRepository.Delete(announcement);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<Announcement>> GetAll() => await _unitOfWork.AnnouncementsRepository.FindAllAsync();

        public async Task<List<Announcement>> GetAllByBuildingId(int buildingId) =>
            await _unitOfWork.AnnouncementsRepository.FindAllByTargetBuildingIdAsync(buildingId);
        
        public async Task<List<Announcement>> GetAllByAddress(Address address)
        {
            if (address is null) 
                throw new BadRequestException("Address must not be null");

            return await _unitOfWork.AnnouncementsRepository.FindAllByAddressAsync(address);
        }

        public async Task<List<AnnouncementDto>> GetAllByReceiverId(int receiverId)
        {
            var receiver = await _unitOfWork.UsersRepository.FindByIdAndIncludeAllLocalsAsync(receiverId) 
                           ?? throw new NotFoundException();

            var receiverLocals = receiver.ResidedLocals
                .Concat(receiver.OwnedLocals)
                .Distinct()
                .ToList();
            
            List<Announcement> announcements = new();

            foreach (var local in receiverLocals)
            {
                var b = await _unitOfWork.BuildingsRepository.FindByIdWithAddressAsync(local.BuildingId);
                if (b is not null)
                {
                    var buildingAnnouncements =
                        await _unitOfWork.AnnouncementsRepository.FindAllByTargetBuildingIdAsync(local.BuildingId);
                    announcements = announcements.Concat(buildingAnnouncements).Distinct().ToList();
                }
            }
            return GetAnnouncementsAsDtos(announcements);
        }

        public async Task<List<AnnouncementDto>> GetAllByAuthorId(int authorId)
        {
            var announcements =  await _unitOfWork.AnnouncementsRepository.FindAllByAuthorIdAsync(authorId);
            return (announcements is not null) ? GetAnnouncementsAsDtos(announcements) : null;
        }

        public async Task CancelAnnouncementById(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindByIdAsync(id) 
                               ?? throw new NotFoundException();

            announcement.Cancelled = DateTimeOffset.Now;
            
            _unitOfWork.AnnouncementsRepository.Update(announcement);
            await _unitOfWork.CommitAsync();
        }

        // public async Task<List<AnnouncementDto>> GetAllFiltered(AnnouncementsFilterRequest filter)
        // {
        //     if (filter is null)
        //         throw new BadRequestException();
        //     filter.Address ??= new Address();
        //     var announcements =  await _unitOfWork.AnnouncementsRepository.FindAllFiltered(filter);
        //     return GetAnnouncementsAsDtos(announcements);
        // }

        private async Task AddAnnouncementWithBuildings(AnnouncementDto announcementDto, List<Building> buildings)
        {
            var announcement = new Announcement
            {
                TargetBuildings = buildings,
                AuthorId = announcementDto.AuthorId,
                Created = DateTimeOffset.Now,
                Content = announcementDto.Content,
                Title = announcementDto.Title,
                ExpirationDate = announcementDto.ExpirationDate,
                Type = announcementDto.Type
            };
            
            if (await _unitOfWork.AnnouncementsRepository.CheckIfExistsAsync(announcement))
                throw new BadRequestException("Such announcement already exists.");
            
            await _unitOfWork.AnnouncementsRepository.AddAsync(announcement);
            await _unitOfWork.CommitAsync();
        }
        
        

        private List<AnnouncementDto> GetAnnouncementsAsDtos(List<Announcement> announcements)
        {
            List<AnnouncementDto> announcementDtos = new();
            announcements.ForEach(a => announcementDtos.Add(a.AsDto()));
            return announcementDtos;
        }
    }
}