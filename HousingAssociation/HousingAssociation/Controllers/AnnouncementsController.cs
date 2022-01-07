using System;
using System.Threading.Tasks;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements() => Ok(await _announcementsService.GetAll());
        
        [Authorize(Roles = "Worker")]
        [HttpGet("not-cancelled")]
        public async Task<IActionResult> GetAllNotCancelledAnnouncements() => Ok(await _announcementsService.GetAll());
        
        [Authorize(Roles = "Resident")]
        [HttpGet("receiver/{receiverId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByReceiverId(int receiverId)
            => Ok(await _announcementsService.GetAllByReceiverId(receiverId));

        [Authorize(Roles = "Worker")]
        [HttpGet("author/{authorId:int}")]
        public async Task<IActionResult> GetAllAnnouncementsByAuthorId(int authorId)
            => Ok(await _announcementsService.GetAllByAuthorId(authorId));
        
        // worker, admin
        // [HttpPost("filter")]
        // public async Task<IActionResult> GetAllByFilters(AnnouncementsFilterRequest filter)
        // {
        //     return Ok(await _announcementsService.GetAllFiltered(filter));
        // }

        [Authorize(Roles = "Worker")]
        [HttpPost("add-by-address")]
        public async Task<IActionResult> AddByAddress(AnnouncementDto announcement)
        {
            await _announcementsService.AddAnnouncementByAddress(announcement);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpPost("add-by-buildings")]
        public async Task<IActionResult> AddByBuildingsIds(AnnouncementDto announcement)
        {
            await _announcementsService.AddAnnouncementByBuildingsIds(announcement);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, AnnouncementDto announcement)
        { 
            announcement.Id = id;
            await _announcementsService.UpdateAnnouncement(announcement);
            return Ok();
        }
        
        [Authorize(Roles = "Worker")]
        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _announcementsService.CancelAnnouncementById(id);
            return Ok();
        }
    }
}