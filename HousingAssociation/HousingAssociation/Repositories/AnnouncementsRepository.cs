using System;
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
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Announcement> _announcements;
        public AnnouncementsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _announcements = dbContext.Announcements;
        }

        public async Task CancelOldAnnouncements()
        {
            await _dbContext.Database
                .ExecuteSqlInterpolatedAsync(
                    $"UPDATE announcements SET expired={DateTimeOffset.Now} WHERE cancelled = NULL AND expired = NULL AND expiration_date <= {DateTimeOffset.Now}");
        }
        public async Task<bool> CheckIfExistsAsync(Announcement announcement)
        {
            var similarAnnouncements =  await _announcements
                .Include(a => a.TargetBuildings)
                .Where(a =>
                    a.Cancelled == null && a.Expired == null &&
                    a.Title.Equals(announcement.Title) &&
                    a.Content.Equals(announcement.Content)
                ).ToListAsync();
            
            if (!similarAnnouncements.Any())
                return false;
            
            var value = similarAnnouncements.Any(a =>
                a.TargetBuildings.Any(b => announcement.TargetBuildings.Any(b1 => b1.Id == b.Id)));

            return value;
        }

        public async Task AddAsync(Announcement announcement) => await _announcements.AddAsync(announcement);
        public void Update(Announcement announcement) => _announcements.Update(announcement);
        public void Delete(Announcement announcement) => _announcements.Remove(announcement);
        public async Task<Announcement> FindByIdAsync(int id) => await _announcements.FindAsync(id);
        public async Task<List<Announcement>> FindAllByAuthorIdAsync(int authorId)
            => await _announcements
                .Where(a => a.AuthorId == authorId)
                .ToListAsync();
        public async Task<List<Announcement>> FindAllAsync() => await _announcements.ToListAsync();
        public async Task<List<Announcement>> FindAllNotCancelledAsync() 
            => await _announcements
                .Include(a => a.Author)
                .Include(a => a.TargetBuildings)
                    .ThenInclude(building => building.Address)
                .Where(a => a.Cancelled == null)
                .ToListAsync();
        public async Task<List<Announcement>> FindNotCancelledByTargetBuildingIdAsync(int buildingId)
            => await _announcements
                .Include(a => a.TargetBuildings)
                .Include(a => a.Author)
                .Where(a => a.TargetBuildings.Any(b => b.Id == buildingId))
                .Where(a => a.Cancelled == null)
                .ToListAsync();
        public async Task<List<Announcement>> FindAllByAddressAsync(Address address) 
            => await _announcements
                .Include(a => a.TargetBuildings)
                .ThenInclude(b => 
                    (string.IsNullOrEmpty(address.City) || b.Address.City.Equals(address.City)) &&
                    (string.IsNullOrEmpty(address.District) || b.Address.District.Equals(address.District)) &&
                    (string.IsNullOrEmpty(address.Street) || b.Address.Street.Equals(address.Street)))
                .ToListAsync();

        // public async Task<List<Announcement>> FindAllFiltered(AnnouncementsFilterRequest filter)
        //     => await _announcements
        //         .Include(a => a.TargetBuildings)
        //         .ThenInclude(b =>
        //             (string.IsNullOrEmpty(filter.Address.City) || b.Address.City.Equals(filter.Address.City)) &&
        //             (string.IsNullOrEmpty(filter.Address.District) || b.Address.District.Equals(filter.Address.District)) &&
        //             (string.IsNullOrEmpty(filter.Address.Street) || b.Address.Street.Equals(filter.Address.Street)))
        //         .Where(a => 
        //             (filter.IsActual == null || a.IsCancelledOrExpired != filter.IsActual) &&
        //             (filter.DateFrom == null || a.CreatedAt >= filter.DateFrom) &&
        //             (filter.DateTo == null || a.CreatedAt <= filter.DateTo))
        //         .ToListAsync();
    }
}