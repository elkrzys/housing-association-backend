using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public record Announcement : Event
    {
        [ForeignKey("PreviousAnnouncement")] public int? PreviousAnnouncementId { get; set; }
        public List<Building> TargetBuildings { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public DateTimeOffset? Cancelled { get; set; }
        public DateTimeOffset? Expired { get; set; }
        
        public Announcement PreviousAnnouncement { get; set; }
    }
}