using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Repositories
{
    public class BuildingsRepository
    {
        private readonly AppDbContext _dbContext;

        public BuildingsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Building building)
        {
            _dbContext.Buildings.Add(building);
            _dbContext.SaveChanges();
            return building.Id;
        }

        public void Update(Building building)
        {
            var oldBuilding = _dbContext.Buildings.Find(building.Id);

            if (oldBuilding is not null)
            {
                _dbContext.Buildings.Update(building with {Id = oldBuilding.Id});
                _dbContext.SaveChanges();
            }
        }
    }
}