using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;

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
            building = await _unitOfWork.BuildingsRepository.Add(building with {Address = address});
            _unitOfWork.Commit();

            return building;
        }
    }
}