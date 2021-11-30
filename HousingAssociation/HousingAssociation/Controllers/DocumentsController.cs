using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase
    {
        // private readonly DocumentsService _documentsService;
        //
        // public DocumentsController(DocumentsService documentsService)
        // {
        //     _documentsService = documentsService;
        // }

        // admin, worker
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            return Ok();
        }
        
        // admin, worker, resident
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            return Ok();
        }
        
        // admin, worker, resident
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetDocumentByUserId(int userId)
        {
            return Ok();
        }

        // admin, worker, resident
        // [HttpPost]
        // public async Task<IActionResult> AddDocument([FromForm] UploadDocumentRequest request)
        // {
        //     return Ok();
        // }
        
        // admin, worker
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            return Ok();
        }
    }
}