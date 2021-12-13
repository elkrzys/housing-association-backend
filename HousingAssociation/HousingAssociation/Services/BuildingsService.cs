using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;

namespace HousingAssociation.Services
{
    public class BuildingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BuildingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Building>> GetAll() => await _unitOfWork.BuildingsRepository.FindAllAsync();
        public async Task<Building> AddBuildingWithAddress(BuildingDto buildingDto)
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

            building = await _unitOfWork.BuildingsRepository.AddAsync(building);
            _unitOfWork.Commit();

            return building;
        }
        public async Task<List<Building>> GetAllBuildingsByAddress(Address address) 
            => await _unitOfWork.BuildingsRepository.FindByAddressAsync(address);
        
        public async Task Update(Building building)
        {
            if (await _unitOfWork.BuildingsRepository.FindByIdAsync(building.Id) is null)
                throw new NotFoundException();

            var address = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(building.Address);
            
            _unitOfWork.BuildingsRepository.Update(building with { Address = address });
            _unitOfWork.Commit();
        }

        public async Task DeleteById(int id)
        {
            var building = await _unitOfWork.BuildingsRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            _unitOfWork.BuildingsRepository.Delete(building);
            _unitOfWork.Commit();
        }
        
    }
}