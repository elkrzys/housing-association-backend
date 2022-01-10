using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentsService _documentsService;
        
        public DocumentsController(DocumentsService documentsService)
        {
            _documentsService = documentsService;
        }
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
            => Ok(await _documentsService.FindAll());
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("author/{authorId:int}")]
        public async Task<IActionResult> GetDocumentsByAuthorId(int authorId)
            => Ok(await _documentsService.FindAllByAuthorId(authorId));

        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("all-from-association")]
        public async Task<IActionResult> GetAllAssociationDocuments()
            => Ok(await _documentsService.FindAllSendByAssociation());
        
        [Authorize(Roles = "Worker")]
        [HttpGet("all-from-residents")]
        public async Task<IActionResult> GetAllFromResidents()
            => Ok(await _documentsService.FindAllSendByResidents());
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpGet("receiver/{receiverId:int}")]
        public async Task<IActionResult> GetDocumentsByReceiverId(int receiverId)
            => Ok(await _documentsService.FindAllByReceiverId(receiverId));

        [Authorize(Roles = "Worker, Resident")]
        [HttpPost]
        public async Task<IActionResult> AddDocument([FromForm] UploadDocumentRequest request)
        {
            await _documentsService.AddNewDocument(request);
            return Ok();
        }
        
        [Authorize(Roles = "Worker, Resident")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentsService.DeleteById(id);
            return Ok();
        }
    }
}