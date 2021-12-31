using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HousingAssociation.Controllers.Requests
{
    public class UploadDocumentRequest
    {
        [Required] public int AuthorId { get; set; }
        [Required] public string Title { get; set; }
        public List<int> ReceiversIds { get; set; }
        [Required] public IFormFile DocumentFile { get; set; }
        public DateTimeOffset? Removes { get; set; }
    }
}