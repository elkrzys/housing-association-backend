using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly BuildingsService _buildingsService;

        public BuildingsController(BuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }
        
        //TODO: make this controller available only for worker and admin
        [HttpGet]
        public async Task<IActionResult> GetAllBuildings() => Ok(await _buildingsService.GetAll());
        
        // TODO: check if params from querystring will be mapped to object (Postman)
        [HttpGet("address")]
        public async Task<IActionResult> GetAllBuildingsByAddress(string city = null, string district = null, string street = null)
            => Ok(await _buildingsService.GetAllBuildingsByAddress(new Address {City = city, District = district, Street = street}));
        
        [HttpPost]
        public async Task<IActionResult> AddBuilding(BuildingDto building)
            => Ok(await _buildingsService.AddBuildingWithAddress(building));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBuilding(int id, Building building)
        {
            await _buildingsService.Update(building with {Id = id});
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            await _buildingsService.DeleteById(id);
            return Ok();
        }
    }
}