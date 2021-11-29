using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;

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
        public async Task<Building> AddBuildingWithAddress(Building building)
        {
            var address = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(building.Address);
            building = await _unitOfWork.BuildingsRepository.AddAsync(building with {Address = address});
            _unitOfWork.Commit();

            return building;
        }
        public async Task<List<Building>> GetAllBuildingsByAddress(Address address)
        {
            var existingAddress = await _unitOfWork.AddressesRepository.FindAddressAsync(address);
            if (existingAddress is not null)
            {
                return await _unitOfWork.BuildingsRepository.FindAllByExistingAddressAsync(existingAddress);
            }
            return await _unitOfWork.BuildingsRepository.FindByNotCompleteAddress(address);
        }

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