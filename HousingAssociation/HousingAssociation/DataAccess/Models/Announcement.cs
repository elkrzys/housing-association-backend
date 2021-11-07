using System;
using System.Collections.Generic;

namespace HousingAssociation.DataAccess.Models
{
    public class Announcement : Event
    {
        public List<Building> TargetBuildings { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsCancelledOrExpired { get; set; }
    }
}