using System.Collections.Generic;
using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
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
        public async Task<ActionResult<List<User>>> GetUnconfirmedUsers()
        {
            return Ok(await _usersService.FindUnconfirmedUsers());
        }
        
    }
}