using System.Collections.Generic;
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

        public async Task<Building> FindByIdAsync(int id) => await _buildings.FindAsync(id);

        public async Task<Building> FindByIdWithLocalsAsync(int id)
            => await _buildings
                .Include(b => b.Locals)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Building> AddAsync(Building building)
        {
            await _buildings.AddAsync(building);
            return building;
        }

        public async Task<List<Building>> FindAllByAddressAsync(Address address) 
            => await _buildings.Include(b => b.Address == address).ToListAsync();

        
    }
}