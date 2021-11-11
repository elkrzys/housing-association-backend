using System.Collections.Generic;

namespace HousingAssociation.DataAccess.Entities
{
    public class UserWall
    {
        public User Owner { get; set; }
        public List<Announcement> AvailableAnnouncements { get; set; }
        public List<Issue> ActiveIssues { get; set; }
    }
}