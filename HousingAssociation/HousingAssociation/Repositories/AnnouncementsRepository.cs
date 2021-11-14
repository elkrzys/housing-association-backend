using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class AnnouncementsRepository
    {
        private readonly DbSet<Announcement> _announcements;

        public AnnouncementsRepository(AppDbContext dbContext)
        {
            _announcements = dbContext.Announcements;
        }

        public async Task Add(Announcement announcement) => await _announcements.AddAsync(announcement);
        
        public async Task Update(Announcement announcement)
        {
            var existingAnnouncement = await _announcements.FindAsync(announcement.Id);
            if (existingAnnouncement is not null)
            {
                _announcements.Update(announcement);
            }
        }

        public void Delete(Announcement announcement) => _announcements.Remove(announcement);

        public async Task<Announcement> FindById(int id) => await _announcements.FindAsync(id);

        public async Task<List<Announcement>> FindAll() => await _announcements.ToListAsync();

        public async Task<List<Announcement>> FindAllByTargetBuildingId(int buildingId) => await _announcements
                .Include(a => a.TargetBuildings.Where(b => b.Id == buildingId))
                .ToListAsync();
        
    }
}