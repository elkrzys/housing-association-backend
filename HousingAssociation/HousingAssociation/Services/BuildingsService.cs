using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils.Extensions;
using Serilog;

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
        
        public async Task<BuildingDto> GetById(int id)
        {
            var building = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(id);
            if (building is null)
            {
                Log.Warning($"Building with id = {id} doesn't exist.");
                throw new BadRequestException($"Building doesn't exist.");
            }
            return building.AsDto();
        }

        public async Task<int> AddBuildingWithAddress(BuildingDto buildingDto)
        {
            var address = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(buildingDto.Address);
            if (address.Id is not 0)
            {
                _unitOfWork.SetModified(address);
            }
            var building = new Building
            {
                Number = buildingDto.Number,
                Type = buildingDto.Type,
                Address = address
            };

            var existingBuildingId = await GetBuildingIdIfExists(building);
            if (existingBuildingId is not null)
            {
                Log.Warning($"Trying to add the same building as building with id = {existingBuildingId}");
                throw new BadRequestException("Building already exists.");
            }

            await _unitOfWork.BuildingsRepository.AddAsync(building);
            await _unitOfWork.CommitAsync();
            return building.Id;
        }
        public async Task<List<BuildingDto>> GetAllBuildingsByAddress(Address address)
        {
            var buildings = await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
            return GetBuildingsAsDtos(buildings);
        }

        public async Task<List<BuildingDto>> GetAllBuildingsByIds(List<int> ids)
        {
            if (!ids.Any())
            {
                Log.Warning("Required buildings ids.");
                throw new BadRequestException("Request without buildings ids.");
            }

            List<BuildingDto> buildingsDtos = new();
            foreach (var id in ids)
            {
                var building = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(id);
                buildingsDtos.Add(building.AsDto());
            }
            return buildingsDtos;
        }

        public async Task Update(BuildingDto buildingDto)
        {
            if (buildingDto.Id is null)
            {
                Log.Warning("Building Id can't be null.");
                throw new BadRequestException("Building Id can't be null");
            }

            var building = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(buildingDto.Id!.Value);
            var newAddress = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(buildingDto.Address);
            
            if (newAddress.Id != 0)
            {
                building.Address = null;
                building.AddressId = newAddress.Id;
            }
            else
            {
                building.Address = newAddress;
            }

            building.Number = buildingDto.Number;
            building.Type = buildingDto.Type;
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteById(int id)
        {
            var building = await _unitOfWork.BuildingsRepository.FindByIdWithDetailsAsync(id);
            if(building is null)
            {
                Log.Warning($"Building with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            _unitOfWork.BuildingsRepository.Delete(building);
            await _unitOfWork.CommitAsync();
        }

        private List<BuildingDto> GetBuildingsAsDtos(IEnumerable<Building> buildings)
            => buildings.Select(building => building.AsDto()).ToList();

        private async Task<int?> GetBuildingIdIfExists(Building building)
        {
            var buildingsWithTheSameAddress =
                await _unitOfWork.BuildingsRepository.FindByAddressAsync(building.Address);
            return buildingsWithTheSameAddress.FirstOrDefault(b => b.Number.Equals(building.Number))?.Id;
        }

    }
}