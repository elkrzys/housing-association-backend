using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.Services;
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

        // admin, worker
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            return Ok(await _documentsService.FindAll());
        }
        
        // admin, worker, resident
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            return Ok(await _documentsService.FindById(id));
        }
        
        // admin, worker, resident
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetDocumentByUserId(int userId)
        {
            return Ok(await _documentsService.FindAllByAuthorId(userId));
        }

        //admin, worker, resident
        [HttpPost]
        public async Task<IActionResult> AddDocument([FromForm] UploadDocumentRequest request)
        {
            await _documentsService.AddNewDocument(request);
            return Ok();
        }
        
        // admin, worker
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentsService.DeleteById(id);
            return Ok();
        }
    }
}