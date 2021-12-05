using System;
using System.Collections.Generic;

namespace HousingAssociation.DataAccess.Entities
{
    public record Announcement : Event
    {
        public List<Building> TargetBuildings { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsCancelledOrExpired { get; set; }
        public int? PreviousAnnouncementId { get; set; }
    }
}