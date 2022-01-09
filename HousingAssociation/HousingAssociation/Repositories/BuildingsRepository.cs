using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class BuildingsRepository
    {
        private readonly DbSet<Building> _buildings;
        public BuildingsRepository(AppDbContext dbContext)
        {
            _buildings = dbContext.Buildings;
        }
        public async Task<List<Building>> FindAllAsync() 
            => await _buildings
                .Include(b => b.Address)
                .Include(b => b.Locals)
                .ToListAsync();

        public async Task<Building> FindByIdAsync(int id) => await _buildings.FindAsync(id);

        public async Task<Building> FindByIdWithDetailsAsync(int id)
            => await _buildings
                .Include(b => b.Address)
                .Include(b => b.Locals)
                .SingleOrDefaultAsync(b => b.Id == id);

        public async Task<bool> CheckIfExistsAsync(Building building)
            => await _buildings
                .Include(b => b.Address)
                .Where(b => b.Address.City.Equals(building.Address.City) &&
                       (string.IsNullOrEmpty(building.Address.District) || b.Address.District.Equals(building.Address.District)) &&
                        b.Address.Street.Equals(building.Address.Street))
                .AnyAsync(b => b.Number == building.Number);
        public async Task<Building> AddAsync(Building building)
        {
            await _buildings.AddAsync(building);
            return building;
        }
        public async Task<List<Building>> FindByAddressAsync(Address address) 
            => await _buildings
                    .Include(b => b.Address)
                    .Include(b => b.Locals)
                    .Where(b =>
                        (string.IsNullOrEmpty(address.City) || b.Address.City.Equals(address.City)) &&
                        (string.IsNullOrEmpty(address.District) || string.IsNullOrWhiteSpace(address.District) || b.Address.District.Equals(address.District)) &&
                        (string.IsNullOrEmpty(address.Street) || b.Address.Street.Equals(address.Street)))
                    .ToListAsync();
        public void Update(Building building) => _buildings.Update(building);
        public void Delete(Building building) => _buildings.Remove(building);
    }
}