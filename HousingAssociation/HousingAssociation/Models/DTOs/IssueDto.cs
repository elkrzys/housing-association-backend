#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public class IssueDto
    {
        public int? Id { get; set; }
        [MaxLength(255)][Required] public string Title { get; set; }
        [MaxLength(255)][Required] public string Content { get; set; }
        [Required] public int AuthorId { get; set; }
        [Required] public int SourceLocalId { get; set; }
        public int? SourceBuildingId { get; set; }
        public DateTimeOffset? Resolved { get; set; }
        public DateTimeOffset? Cancelled { get; set; }
        public Address? Address { get; set; }
    }
}