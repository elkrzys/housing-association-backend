using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class LocalsRepository
    {
        private readonly DbSet<Local> _locals;
        public LocalsRepository(AppDbContext dbContext)
        {
            _locals = dbContext.Locals;
        }
        public async Task<Local> FindByIdAsync(int id) => await _locals.FindAsync(id);
        
        public async Task<Local> AddIfNotExistsAsync(Local local)
        {
            var existingLocal =
                await _locals.FirstOrDefaultAsync(l => l.Number == local.Number && l.BuildingId == local.BuildingId);

            if (existingLocal is not null) return null;
            
            await _locals.AddAsync(local);
            return local;
        }

        public void Update(Local local) => _locals.Update(local);

        public void Delete(Local local) => _locals.Remove(local);

        public async Task<List<Local>> GetAllByBuildingIdAsync(int buildingId)
            => await _locals.Where(local => local.BuildingId == buildingId).ToListAsync();
    }
}