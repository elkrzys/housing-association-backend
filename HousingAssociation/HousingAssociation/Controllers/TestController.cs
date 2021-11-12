using System;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Repositories;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        //private readonly BuildingsRepository _buildingsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BuildingsService _buildingsService;
        private readonly UsersService _usersService;
        private readonly AuthenticationService _authService;
        
        public TestController(IUnitOfWork unitOfWork, BuildingsService buildingsService, UsersService usersService, AuthenticationService authService)
        {
            //_buildingsRepository = buildingsRepository;
            _unitOfWork = unitOfWork;
            _buildingsService = buildingsService;
            _usersService = usersService;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<DateTime> GetCurrentDate()
        {
            return DateTime.Now;
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostBuilding(Building building)
        {
            var added = await _buildingsService.AddBuildingWithAddress(building);
            return Ok(added.Id);
        }
        
        // [HttpPost("register")]
        // public async Task<ActionResult<int>> RegisterUser(User user)
        // {
        //     string password = "haselko";
        //     user = await _authService.RegisterUser(user, password);
        //     return Ok(user.Id);
        // }
        
        // [HttpPost("ech")]
        // public async Task<ActionResult<Address>> PostAddress(Address address)
        // {
        //     //var id = await _unitOfWork.Addresses.AddNewAddressOrReturnExisting(address);
        //     var adr = await _buildingsService.AddNewOrReturnExisting(address);
        //     
        //     return Ok(adr);
        // }
        
        
    }
}