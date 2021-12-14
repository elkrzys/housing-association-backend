using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public class AnnouncementDto
    {
        public int? Id { get; set; }
        [Required] public AnnouncementType Type { get; set; }
        [MaxLength(255)] public string Title { get; set; }
        [MaxLength(255)] public string Content { get; set; }
        [Required] public int AuthorId { get; set; }
        public List<int> TargetBuildingsIds { get; set; }
        
        public List<Address> Addresses { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}