using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;

namespace HousingAssociation.Services
{
    public class AddressesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> GetAllCities()
            => await _unitOfWork.AddressesRepository.FindAllCitiesAsync();

        public async Task<List<string>> GetDistrictsByCity(string city)
            => await _unitOfWork.AddressesRepository.FindAllDistrictsByCityAsync(city);

        public async Task<List<string>> GetStreetsByCityAndDistrict(string city, string district)
            => await _unitOfWork.AddressesRepository.FindAllStreetsByCityAndDistrictAsync(city, district);


    }
}