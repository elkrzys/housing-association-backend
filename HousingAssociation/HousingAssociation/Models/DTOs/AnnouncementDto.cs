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
        
        public int? PreviousAnnouncementId { get; set; }
        [MaxLength(255)] public string Title { get; set; }
        [MaxLength(255)] public string Content { get; set; }
        public DateTimeOffset Created { get; set; }
        public Author Author { get; set; }
        public List<int> TargetBuildingsIds { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}