using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
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

        public async Task<List<Announcement>> FindAllByAuthorId(int authorId)
            => await _announcements
                .Where(a => a.AuthorId == authorId)
                .ToListAsync();

        public async Task<List<Announcement>> FindAll() => await _announcements.ToListAsync();

        public async Task<List<Announcement>> FindAllByTargetBuildingId(int buildingId) => await _announcements
                .Include(a => a.TargetBuildings)
                .Where(b => b.Id == buildingId)
                .ToListAsync();

        public async Task<List<Announcement>> FindAllByAddress(Address address) 
            => await _announcements
                .Include(a => a.TargetBuildings)
                .ThenInclude(b => (string.IsNullOrEmpty(address.City) || b.Address.City.Equals(address.City)) &&
                                  (string.IsNullOrEmpty(address.District) || b.Address.District.Equals(address.District)) &&
                                  (string.IsNullOrEmpty(address.Street) || b.Address.Street.Equals(address.Street)))
                .ToListAsync();

        public async Task<List<Announcement>> FindAllFiltered(AnnouncementsFilterRequest filter)
            => await _announcements
                .Include(a => a.TargetBuildings)
                .ThenInclude(b =>
                    (string.IsNullOrEmpty(filter.Address.City) || b.Address.City.Equals(filter.Address.City)) &&
                    (string.IsNullOrEmpty(filter.Address.District) || b.Address.District.Equals(filter.Address.District)) &&
                    (string.IsNullOrEmpty(filter.Address.Street) || b.Address.Street.Equals(filter.Address.Street)))
                .Where(a => 
                    (filter.IsActual == null || a.IsCancelledOrExpired != filter.IsActual) &&
                    (filter.DateFrom == null || a.CreatedAt >= filter.DateFrom) &&
                    (filter.DateTo == null || a.CreatedAt <= filter.DateTo))
                .ToListAsync();
    }
}