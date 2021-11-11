using System.Collections.Generic;

namespace HousingAssociation.DataAccess.Entities
{
    public class WorkerWall
    {
        public List<Announcement> Announcements { get; set; }
        public List<Issue> PendingIssues { get; set; }
    }
}