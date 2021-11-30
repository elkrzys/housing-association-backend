using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("announcements")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly AnnouncementsService _announcementsService;
        public AnnouncementsController(AnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;
        }

        // worker, admin
        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            return Ok();
        }
        // worker, admin, resident
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByUserId(int userId)
        {
            return Ok();
        }
        // worker, admin
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllByFilters(AnnouncementsFilterRequest filter)
        {
            return Ok();
        }
        // resident
        [HttpPost("{userId:int}/filter")]
        public async Task<IActionResult> GetAllByUserIdAndFilters(int userId, AnnouncementsFilterRequest filter)
        {
            return Ok();
        }
        // worker, admin
        [HttpPost]
        public async Task<IActionResult> Add(Announcement announcement)
        {
            return Ok();
        }
        // worker, admin
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Announcement announcement)
        {
            // TODO: set old announcement as cancelled and create new one
            return Ok();
        }
        // worker, admin
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            // TODO: set announcement as cancelled
            return Ok();
        }
    }
}