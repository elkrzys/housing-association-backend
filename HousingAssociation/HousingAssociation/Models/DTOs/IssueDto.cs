#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public class Author
    {
        [Required] public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
    public class IssueDto
    {
        public int? Id { get; set; }
        [MaxLength(255)][Required] public string Title { get; set; }
        [MaxLength(255)][Required] public string Content { get; set; }
        public string? Created { get; set; }
        [Required] public int SourceLocalId { get; set; }
        public int? SourceBuildingId { get; set; }
        
        public string? LocalNumber { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Resolved { get; set; }
        public string? Cancelled { get; set; }
        public Address? Address { get; set; }
        public Author? Author { get; set; }
    }
}