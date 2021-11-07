using System.Collections.Generic;
using DataAccess.Context;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class EventsRepository
    {
        private readonly IDbContext _dbContext;
        public EventsRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Event> FindAll()
        {
            return new List<Event>();
        }

        public Event FindById(int eventId)
        {
            
            return new Issue(); // Announcement
        }

        public IEnumerable<Event> FindAllByAuthorId(int authorId)
        {
            return null;
        }

        public IEnumerable<Event> FindAllByBlockId(int blockId)
        {
            return null;
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