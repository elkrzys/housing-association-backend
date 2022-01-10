using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
            => Ok(await _usersService.FindUserById(id));
        
        [Authorize(Roles = "Worker")]
        [HttpGet("residents")]
        public async Task<ActionResult<List<UserDto>>> GetResidents()
            => Ok(await _usersService.FindAllResidents());

        [Authorize(Roles = "Worker")]
        [HttpGet("workers")]
        public async Task<ActionResult<List<UserDto>>> GetWorkers()
            => Ok(await _usersService.FindAllWorkers());

        [Authorize(Roles = "Worker")]
        [HttpPost("workers")]
        public async Task<IActionResult> AddNewWorker(UserDto userDto)
        {
            return Ok(await _usersService.AddWorker(userDto));
        }

        [Authorize(Roles = "Worker, Resident")]
        [HttpPut("unregister/{id:int}")]
        public async Task<IActionResult> UnregisterUser(int id, [FromBody] string password)
        {
            await _usersService.DisableUser(id, password, true);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpPut("ban/{id:int}")]
        public async Task<IActionResult> BanUser(int id)
        {
            await _usersService.DisableUser(id);
            return Ok();
        }

        [Authorize(Roles = "Worker, Resident")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserDto user)
        {
            await _usersService.Update(user with {Id = id});
            return Ok();
        }

        [Authorize(Roles = "Worker, Resident")]
        [HttpPut("{id:int}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordRequest request)
        {
            await _usersService.ChangePassword(id, request);
            return Ok();
        }
    }
}