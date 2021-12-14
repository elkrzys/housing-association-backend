using System;
using Newtonsoft.Json;

namespace HousingAssociation.Models.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string FilePath { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Removes { get; set; }
    }
}