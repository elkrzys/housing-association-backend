using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Models.DTOs;
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
        public async Task<IActionResult> GetAllAnnouncements() => Ok(_announcementsService.GetAll());
        
        // resident
        [HttpGet("{receiverId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByReceiverId(int receiverId)
            => Ok(await _announcementsService.GetAllByReceiverId(receiverId));

        // worker, admin
        [HttpGet("{authorId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByAuthorId(int authorId)
            => Ok(await _announcementsService.GetAllByAuthorId(authorId));
        
        // worker, admin
        // [HttpPost("filter")]
        // public async Task<IActionResult> GetAllByFilters(AnnouncementsFilterRequest filter)
        // {
        //     return Ok(await _announcementsService.GetAllFiltered(filter));
        // }

        // worker, admin
        [HttpPost("add-by-address")]
        public async Task<IActionResult> AddByAddress(AnnouncementDto announcement)
        {
            return Ok(_announcementsService.AddAnnouncementByAddress(announcement));
        }
        
        [HttpPost("add-by-buildings")]
        public async Task<IActionResult> AddByBuildingsIds(AnnouncementDto announcement)
        {
            await _announcementsService.AddAnnouncementByBuildingsIds(announcement);
            return Ok();
        }
        
        // worker, admin
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, AnnouncementDto announcement)
        { 
            announcement.Id = id;
            return Ok(_announcementsService.UpdateAnnouncement(announcement));
        }
        // worker, admin
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _announcementsService.CancelAnnouncementById(id);
            return Ok();
        }
    }
}