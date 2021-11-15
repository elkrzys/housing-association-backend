﻿using System;
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
        
        public async Task<Local> AddIfNotExistAsync(Local local)
        {
            var existingLocal =
                await _locals.FirstOrDefaultAsync(l => l.Number == local.Number && l.BuildingId == local.BuildingId);

            if (existingLocal is null)
            {
                await _locals.AddAsync(local);
                return local;
            }
            return null;
        }

        public async Task<IQueryable<Local>> AddRangeIfNotExistsAsync(List<Local> locals)
        {
            await _locals.AddRangeAsync(locals);
            return locals.AsQueryable();
        }

        public async Task<Local> Update(Local local)
        {
            // var existingLocal =  await _locals.FirstOrDefaultAsync(l => l.Number == local.Number && l.BuildingId == local.BuildingId);
            var existingLocal = await _locals.FindAsync(local.Id);

            if (existingLocal is null)
            {
                return null;
            }

            _locals.Update(local);
            return local;
        }

        public async Task<List<Local>> GetAllByBuildingIdAsync(int buildingId)
            => await _locals.Where(local => local.BuildingId == buildingId).ToListAsync();
    }
}