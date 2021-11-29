using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Services;
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

        [HttpGet("{buildingId:int}")]
        public async Task<IActionResult> GetAllByBuildingId(int buildingId)
            => Ok(await _localsService.FindAllFromBuilding(buildingId));

        [HttpPost]
        public async Task<IActionResult> AddLocals(IEnumerable<Local> locals)
        {
            await _localsService.AddLocals(locals.ToList());
            return Ok();
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLocal(int id, Local local)
        {
            await _localsService.UpdateLocal(local with {Id = id});
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            await _localsService.DeleteLocalById(id);
            return Ok();
        }
    }
}