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

        public async Task<Building> AddBuildingWithAddress(Building building)
        {

            var address = await _unitOfWork.AddressesRepository.AddNewAddressOrReturnExisting(building.Address);
            building = await _unitOfWork.BuildingsRepository.AddAsync(building with {Address = address});
            _unitOfWork.Commit();

            return building;
        }

        public async Task<List<Building>> GetAllBuildingsByAddress(Address address)
        {
            var existingAddress = await _unitOfWork.AddressesRepository.FindAddressAsync(address) 
                                  ?? throw new BadRequestException("Address doesn't exists.");

            return await _unitOfWork.BuildingsRepository.FindAllByAddressAsync(existingAddress);
        }
    }
}