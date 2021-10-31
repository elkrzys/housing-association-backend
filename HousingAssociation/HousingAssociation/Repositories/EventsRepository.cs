using System.Collections.Generic;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class EventsRepository
    {
        public EventsRepository()
        {
                
        }

        public List<Event> FindAll()
        {
            return new List<Event>();
        }

        public Event FindById(int eventId)
        {
            
            return new Issue(); // Announcement
        }

        public int Add(Event e)
        {
            int id = 0;
            return id;
        }

        public void Update(Event e)
        {
            
        }

        public void Delete(int eventId)
        {
            
        }

        public void ResolveIssue(int issueId)
        {
            
        }

        public void CloseAnnouncement(int announcementId)
        {
            
        }
    }
}