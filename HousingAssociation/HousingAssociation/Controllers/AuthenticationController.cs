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
        public async Task Register(RegisterRequest request)
        {
            await _authService.RegisterUser(request);
        }
    }
}