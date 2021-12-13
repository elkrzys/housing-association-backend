using System;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
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
            _unitOfWork = unitOfWork;
            _buildingsService = buildingsService;
            _usersService = usersService;
            _authService = authService;
        }

        [HttpGet("test-resident")]
        [Authorize(Roles = "Resident")]
        public ActionResult<DateTime> TestResidentRole()
        {
            return DateTime.Now;
        }
        
        [HttpGet("test-worker")]
        [Authorize(Roles = "Worker")]
        public ActionResult<DateTime> TestWorkerRole()
        {
            return DateTime.Now;
        }
    }
}