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

        [HttpGet("not-enabled")]
        public async Task<ActionResult<List<UserDto>>> GetUnconfirmedUsers() 
            => Ok(await _usersService.FindUnconfirmedUsers());
        

        [HttpPost("add-worker")]
        public async Task<IActionResult> AddNewWorker(RegisterRequest request)
        {
            return null;
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usersService.DeleteUser(id);
            return Ok();
        }
    }
}