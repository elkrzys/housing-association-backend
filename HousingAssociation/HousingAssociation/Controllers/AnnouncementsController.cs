using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllAnnouncements() => Ok(await _announcementsService.GetAll());
        
        // resident
        [HttpGet("receiver/{receiverId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByReceiverId(int receiverId)
            => Ok(await _announcementsService.GetAllByReceiverId(receiverId));

        // worker, admin
        [HttpGet("author/{authorId:int}")]
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
            await _announcementsService.AddAnnouncementByAddress(announcement);
            return Ok();
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
            await _announcementsService.UpdateAnnouncement(announcement);
            return Ok();
        }
        
        // worker, admin
        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _announcementsService.CancelAnnouncementById(id);
            return Ok();
        }
    }
}