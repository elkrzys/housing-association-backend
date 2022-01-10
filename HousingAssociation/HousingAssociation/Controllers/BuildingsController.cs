using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Authorize(Roles = "Worker")]
    [Route("buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly BuildingsService _buildingsService;

        public BuildingsController(BuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _buildingsService.GetById(id));

        [HttpGet]
        public async Task<IActionResult> GetAllBuildings() => Ok(await _buildingsService.GetAll());
        
        [HttpGet("address/{city}/{street}/{district?}")]
        public async Task<IActionResult> GetAllBuildingsByAddress(string city = "", string street = "", string district = "")
            => Ok(await _buildingsService.GetAllBuildingsByAddress(new Address {City = city, District = district, Street = street}));
        
        [HttpPost("get-all-by-ids")]
        public async Task<IActionResult> GetAllBuildingsByIds(List<int> buildingsIds)
            => Ok(await _buildingsService.GetAllBuildingsByIds(buildingsIds));
        
        [HttpPost]
        public async Task<IActionResult> AddBuilding(BuildingDto building)
        {
            await _buildingsService.AddBuildingWithAddress(building);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBuilding(int id, BuildingDto building)
        {
            building.Id = id;
            await _buildingsService.Update(building);
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