using System.Collections.Generic;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models
{
    public class UserWall
    {
        public User Owner { get; set; }
        public List<Announcement> AvailableAnnouncements { get; set; }
        public List<Issue> ActiveIssues { get; set; }
    }
}