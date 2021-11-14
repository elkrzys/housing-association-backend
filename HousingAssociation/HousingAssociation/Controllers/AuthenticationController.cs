using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authService;
        public AuthenticationController(AuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(RegisterRequest request)
        {
            return await _authService.RegisterUser(request);
        }
        
        [HttpPost("login")]
        public async Task Login(LoginRequest request)
        {
            await _authService.Login(request);
        }
    }
}