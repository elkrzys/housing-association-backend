using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Serilog;

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
            {
                Log.Warning("Bad request: AddAnnouncementByBuildingsIds - AnnouncementDto is null.");
                throw new BadRequestException("Announcement incorrect");
            }

            List<Building> buildings = new();
            foreach (var buildingId in announcementDto.TargetBuildingsIds)
            {
                var building = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(buildingId);
                if (building is not null)
                {
                    _unitOfWork.SetModified(building);
                    buildings.Add(building);
                }
            }
            if (!buildings.Any())
            {
                Log.Warning("Bad request: no existing buildings matching the request.");
                throw new BadRequestException("Announcement must have target buildings");
            }
            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }
        
        public async Task AddAnnouncementByAddress(AnnouncementDto announcementDto)
        {
            if (!announcementDto.Addresses.Any())
            {
                Log.Warning("Bad request: Add by address but address list is empty.");
                throw new BadRequestException("No target address defined");
            }

            List<Building> buildings = new();
            foreach (var address in announcementDto.Addresses)
            {
                var buildingsFromAddress = await _unitOfWork.BuildingsRepository.FindByAddressAsync(new Address() with
                {
                    City = address.City,
                    District = address.District,
                    Street = address.Street
                });
                buildings = buildings.Concat(buildingsFromAddress).Distinct().ToList();
            }
            announcementDto.Created = DateTimeOffset.Now;
            _unitOfWork.SetModified(buildings);
            await AddAnnouncementWithBuildings(announcementDto, buildings);
        }

        public async Task UpdateAnnouncement(AnnouncementDto announcementDto)
        {
            if (announcementDto.Id is null)
            {
                Log.Warning("Bad request: announcement Id is null.");
                throw new BadRequestException("Announcement incorrect");
            }

            var cancelledId = announcementDto.Id!.Value;
            announcementDto.Id = null;
            announcementDto.PreviousAnnouncementId = cancelledId;

            await _unitOfWork.OuterTransaction(async () =>
            {
                await CancelAnnouncementById(cancelledId);
                if (announcementDto.TargetBuildingsIds.Any())
                {
                    await AddAnnouncementByBuildingsIds(announcementDto);
                }
                else if (announcementDto.Addresses.Any())
                {
                    await AddAnnouncementByAddress(announcementDto);
                }
                else
                {
                    var buildings = await _unitOfWork.BuildingsRepository.FindAllAsync();
                    await AddAnnouncementWithBuildings(announcementDto, buildings);
                }
            });
        }

        public async Task DeleteAnnouncement(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindByIdAsync(id);
            if (announcement is null)
            {
                Log.Warning($"Announcement with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            _unitOfWork.AnnouncementsRepository.Delete(announcement);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<AnnouncementDto>> GetAll()
        {
            var announcements = await _unitOfWork.AnnouncementsRepository.FindAllNotCancelledAsync();
            return GetAnnouncementsAsDtos(announcements);
        }

        public async Task<List<AnnouncementDto>> GetAllByBuildingId(int buildingId)
        {
            if (await _unitOfWork.BuildingsRepository.FindByIdAsync(buildingId) is null)
            {
                Log.Warning($"Building with id = {buildingId} doesn't exist.");
                throw new NotFoundException();
            }
            var announcements = await _unitOfWork.AnnouncementsRepository.FindNotCancelledByTargetBuildingIdAsync(buildingId);
            return GetAnnouncementsAsDtos(announcements);
        }
        
        public async Task<List<AnnouncementDto>> GetAllByAddress(Address address)
        {
            if (address is null)
            {
                Log.Warning("Attempt to find announcement with address = null.");
                throw new BadRequestException("Address must not be null");
            }
            var announcements = await _unitOfWork.AnnouncementsRepository.FindAllByAddressAsync(address);
            return GetAnnouncementsAsDtos(announcements);
        }

        public async Task<List<AnnouncementDto>> GetAllByReceiverId(int receiverId)
        {
            var receiver = await _unitOfWork.UsersRepository.FindByIdAndIncludeAllLocalsAsync(receiverId); 
            if(receiver is null){               
                Log.Warning($"User with id = {receiverId} doesn't exists.");
                throw new NotFoundException();
            }

            var receiverLocals = receiver.ResidedLocals;
            
            List<Announcement> announcements = new();
            foreach (var local in receiverLocals)
            {
                var b = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(local.BuildingId);
                if (b is not null)
                {
                    var buildingAnnouncements =
                        await _unitOfWork.AnnouncementsRepository.FindNotCancelledByTargetBuildingIdAsync(local.BuildingId);
                    announcements = announcements.Concat(buildingAnnouncements).Distinct().ToList();
                }
            }
            return GetAnnouncementsAsDtos(announcements);
        }

        public async Task<List<AnnouncementDto>> GetAllByAuthorId(int authorId)
        {
            if (await _unitOfWork.UsersRepository.FindByIdAsync(authorId) is null)
            {
                Log.Warning($"User with id = {authorId} doesn't exist.");
                throw new NotFoundException();
            }
            var announcements =  await _unitOfWork.AnnouncementsRepository.FindAllByAuthorIdAsync(authorId);
            return (announcements is not null) ? GetAnnouncementsAsDtos(announcements) : null;
        }

        public async Task CancelAnnouncementById(int id)
        {
            var announcement = await _unitOfWork.AnnouncementsRepository.FindByIdAsync(id);
            if (announcement is null)
            {
                Log.Warning($"Announcement with id = {id} doesn't exists.");
                throw new NotFoundException();
            }

            announcement.Cancelled = DateTimeOffset.Now;
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
                AuthorId = announcementDto.Author.Id,
                Created = DateTimeOffset.Now,
                Content = announcementDto.Content,
                Title = announcementDto.Title,
                ExpirationDate = announcementDto.ExpirationDate,
                Type = announcementDto.Type,
                PreviousAnnouncementId = announcementDto.PreviousAnnouncementId
            };
            
            if (await _unitOfWork.AnnouncementsRepository.CheckIfExistsAsync(announcement))
            {
                Log.Warning("Announcement already exists.");
                throw new BadRequestException("Such announcement already exists.");
            }
            
            await _unitOfWork.AnnouncementsRepository.AddAsync(announcement);
            await _unitOfWork.CommitAsync();
        }
        
        private List<AnnouncementDto> GetAnnouncementsAsDtos(List<Announcement> announcements)
            => announcements.Select(a => a.AsDto()).ToList();
  
    }
}