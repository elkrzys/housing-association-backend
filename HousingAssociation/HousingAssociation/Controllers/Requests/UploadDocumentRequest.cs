using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HousingAssociation.Controllers.Requests
{
    public class UploadDocumentRequest
    {
        [Required] public int AuthorId { get; set; }
        [Required] public string Title { get; set; }
        [Required] public List<int> ReceiversIds { get; set; }
        [Required] public IFormFile Document { get; set; }
        public int? DaysToExpire { get; set; }
    }
}