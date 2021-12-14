using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Entities
{
    public record Announcement : Event
    {
        [Required] public AnnouncementType Type { get; set; }
        public List<Building> TargetBuildings { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsCancelledOrExpired { get; set; }
        public int? PreviousAnnouncementId { get; set; }
    }
}