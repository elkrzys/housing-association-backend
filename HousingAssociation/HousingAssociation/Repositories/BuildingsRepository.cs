using System;
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

        public async Task<Building> Add(Building building)
        {
            await _buildings.AddAsync(building);// with { Address = address});
            return building;
        }

        // public void Update(Building building)
        // {
        //     var oldBuilding = _dbContext.Buildings.Find(building.Id);
        //
        //     if (oldBuilding is not null)
        //     {
        //         _dbContext.Buildings.Update(building with {Id = oldBuilding.Id});
        //         _dbContext.SaveChanges();
        //     }
        // }
    }
}