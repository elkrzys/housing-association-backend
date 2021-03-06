using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("locals")]
    public class LocalsController : ControllerBase
    {
        private readonly LocalsService _localsService;

        public LocalsController(LocalsService localsService)
        {
            _localsService = localsService;
        }

        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("get-by-building/{buildingId:int}")]
        public async Task<IActionResult> GetAllByBuildingId(int buildingId)
            => Ok(await _localsService.FindAllFromBuilding(buildingId));
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("get-by-resident/{residentId:int}")]
        public async Task<IActionResult> GetAllByResidentId(int residentId)
            => Ok(await _localsService.GetAllByResidentId(residentId));
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpPost("get-by-details")]
        public async Task<IActionResult> GetLocalIdByDetails(LocalDto localDto)
            => Ok(await _localsService.GetLocalIdByLocalDetails(localDto));

        [Authorize(Roles = "Worker")]
        [HttpPost]
        public async Task<IActionResult> AddLocal(LocalDto local)
        {
            await _localsService.AddLocal(local);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLocal(int id, LocalDto local)
        {
            local.Id = id;
            await _localsService.UpdateLocal(local);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            await _localsService.DeleteLocalById(id);
            return Ok();
        }
        
        [Authorize(Roles = "Resident")]
        [HttpPost("add-resident/{localId:int}/{residentId:int}")]
        public async Task<IActionResult> AddLocalResident(int localId, int residentId)
        {
            await _localsService.AddResidentToLocal(localId, residentId);
            return Ok();
        }
        
        [Authorize(Roles = "Resident")]
        [HttpDelete("remove-resident/{localId:int}/{residentId:int}")]
        public async Task<IActionResult> RemoveLocalResident(int localId, int residentId)
        {
            await _localsService.RemoveResidentFromLocal(localId, residentId);
            return Ok();
        }
    }
}