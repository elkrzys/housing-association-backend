using System.Threading.Tasks;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("addresses")]
    [Authorize(Roles = "Worker,Resident")]
    public class AddressesController : ControllerBase
    {
        private readonly AddressesService _addressesService;

        public AddressesController(AddressesService addressesService)
        {
            _addressesService = addressesService;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities() => Ok(await _addressesService.GetAllCities());
        
        [HttpGet("districts/{city}")]
        public async Task<IActionResult> GetDistricts(string city) => Ok(await _addressesService.GetDistrictsByCity(city));
        
        [HttpGet("streets/{city}/{district?}")]
        public async Task<IActionResult> GetCitiesStreets(string city, string district = "") 
            => Ok(await _addressesService.GetStreetsByCityAndDistrict(city, district));
        
    }
}