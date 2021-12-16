using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
            => Ok(await _usersService.FindUserById(id));
        
        [HttpGet("not-enabled")]
        public async Task<ActionResult<List<UserDto>>> GetUnconfirmedUsers() 
            => Ok(await _usersService.FindUnconfirmedUsers());
        
        [HttpGet("residents")]
        public async Task<ActionResult<List<UserDto>>> GetResidents() 
            => Ok(await _usersService.FindAllResidents());
        
        [HttpGet("workers")]
        public async Task<ActionResult<List<UserDto>>> GetWorkers() 
            => Ok(await _usersService.FindAllWorkers());

        [HttpPost("add-worker")]
        public async Task<IActionResult> AddNewWorker(UserDto userDto)
        {
            await _usersService.AddWorker(userDto);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserDto user)
        {
            await _usersService.Update(user with {Id = id});
            return Ok();
        }

        [HttpPut("confirm/{id:int}")]
        public async Task<IActionResult> ConfirmUser(int id)
        {
            return Ok(await _usersService.ConfirmUser(id));
        }

        [HttpPut("{id:int}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordRequest request)
        {
            await _usersService.ChangePassword(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usersService.DeleteUser(id);
            return Ok();
        }
    }
}