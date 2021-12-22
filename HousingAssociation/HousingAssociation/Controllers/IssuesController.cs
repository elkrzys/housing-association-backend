using System.Threading.Tasks;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("issues")]
    public class IssuesController : ControllerBase
    {
        private readonly IssuesService _issuesService;

        public IssuesController(IssuesService issuesService)
        {
            _issuesService = issuesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotCancelled() => Ok(await _issuesService.GetAllActual());
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _issuesService.GetById(id));
        
        [HttpGet("author/{authorId:int}")]
        public async Task<IActionResult> GetByAuthorId(int authorId) => Ok(await _issuesService.GetAllByAuthorId(authorId));
        
        [HttpPost]
        public async Task<IActionResult> AddIssue(IssueDto issue)
        {
            await _issuesService.AddIssue(issue);
            return Ok();
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateIssue(int id, IssueDto issueDto)
        {
            issueDto.Id = id;
            await _issuesService.UpdateIssue(issueDto);
            return Ok();
        }

        [HttpPut("resolve/{id:int}")]
        public async Task<IActionResult> ResolveIssue(int id)
        {
            await _issuesService.ResolveIssue(id);
            return Ok();
        }

        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> CancelIssue(int id)
        {
            await _issuesService.CancelIssue(id);
            return Ok();
        }
    }
}