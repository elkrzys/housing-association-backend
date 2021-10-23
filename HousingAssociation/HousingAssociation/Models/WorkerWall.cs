using System.Collections.Generic;

namespace HousingAssociation.Models
{
    public class WorkerWall
    {
        public List<Announcement> Announcements { get; set; }
        public List<Issue> PendingIssues { get; set; }
    }
}