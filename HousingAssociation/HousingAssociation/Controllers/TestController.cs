using System;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<DateTime> GetCurrentDate()
        {
            return DateTime.Now;
        }
    }
}