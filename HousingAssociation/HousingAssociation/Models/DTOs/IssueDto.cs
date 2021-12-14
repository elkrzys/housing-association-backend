using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.Models.DTOs
{
    public class IssueDto
    {
        public int? Id { get; set; }
        [MaxLength(255)] public string Title { get; set; }
        [MaxLength(255)] public string Content { get; set; }
        [Required] public int AuthorId { get; set; }
        public int? SourceLocalId { get; set; }
        public int SourceBuildingId { get; set; }
        public bool? IsResolved { get; set; }
        public bool? IsCancelled { get; set; }
    }
}