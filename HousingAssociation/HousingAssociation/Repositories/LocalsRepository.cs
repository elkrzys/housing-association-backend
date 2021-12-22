using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Models.DTOs;
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

        public async Task<bool> CheckIfExists(Local local)
            => await _locals
                .AnyAsync(l => 
                    string.IsNullOrEmpty(local.Number) || l.Number.Equals(local.Number) &&
                    l.BuildingId.Equals(local.BuildingId));
        
        public async Task<Local> FindByIdAsync(int id)
            => await _locals
                .Include(local => local.Residents)
                .SingleOrDefaultAsync(local => local.Id == id);

        public async Task<List<Local>> FindAllByIdsAsync(List<int> ids)
            => await _locals
                .Where(local => ids.Any(id => local.Id == id))
                .ToListAsync();

        public async Task<List<Local>> FindAllByResidentId(int id)
            => await _locals
                .Include(local => local.Building)
                .ThenInclude(building => building.Address)
                .Include(local => local.Residents)
                .Where(local => local.Residents.Any(r => r.Id == id))
                .ToListAsync();

        public async Task<List<Local>> FindAllByBuildingIdAsync(int buildingId)
            => await _locals
                .Include(local => local.Residents)
                .Where(local => local.BuildingId == buildingId).ToListAsync();
        public async Task AddAsync(Local local) => await _locals.AddAsync(local);
        public void Update(Local local) => _locals.Update(local);
        public void Delete(Local local) => _locals.Remove(local);
    }

}