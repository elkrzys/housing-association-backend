using System.Collections.Generic;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models
{
    public class WorkerWall
    {
        public List<Announcement> Announcements { get; set; }
        public List<Issue> PendingIssues { get; set; }
    }
}