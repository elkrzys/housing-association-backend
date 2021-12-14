using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;

namespace HousingAssociation.Services
{
    public class BuildingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BuildingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<BuildingDto>> GetAll()
        {
            var buildings = await _unitOfWork.BuildingsRepository.FindAllAsync();
            return GetBuildingsAsDtos(buildings);
        }

        public async Task AddBuildingWithAddress(BuildingDto buildingDto)
        {
            var address = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(buildingDto.Address);
            var building = new Building
            {
                Number = buildingDto.Number,
                Type = buildingDto.Type
            };
            
            if (address.Id is not 0)
            {
                building.AddressId = address.Id;
            }
            else
            {
                building.Address = address;
            }

            await _unitOfWork.BuildingsRepository.AddAsync(building);
            await _unitOfWork.CommitAsync();
        }
        public async Task<List<BuildingDto>> GetAllBuildingsByAddress(Address address)
        {
            var buildings = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
            return GetBuildingsAsDtos(buildings);
        }

        public async Task Update(BuildingDto buildingDto)
        {
          
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteById(int id)
        {
            var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            _unitOfWork.BuildingsRepository.Delete(building);
            await _unitOfWork.CommitAsync();
        }

        private List<BuildingDto> GetBuildingsAsDtos(List<Building> buildings)
        {
            List<BuildingDto> buildingDtos = new();
            buildings.ForEach(building => buildingDtos.Add(building.AsDto()));
            return buildingDtos;
        }
        
    }
}