using System;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Controllers.Requests
{
    public class AnnouncementsFilterRequest
    {
        public Address? Address { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        public bool? IsActual { get; set; }
    }
}

