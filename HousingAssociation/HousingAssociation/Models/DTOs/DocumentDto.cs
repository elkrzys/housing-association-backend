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
        public DateTime CreatedAt { get; set; }
        public DateTime? RemovesAt { get; set; }
    }
}