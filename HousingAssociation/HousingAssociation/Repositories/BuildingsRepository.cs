using System.Collections.Generic;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class BuildingsRepository
    {
        public BuildingsRepository()
        {
                
        }

        public List<Building> FindAll()
        {
            return new List<Building>();
        }

        public Building FindById(int buildingId)
        {
            return new Building();
        }

        public int Add(Building building)
        {
            int id = 0;
            return id;
        }

        public void Update(Building building)
        {
            
        }

        public void Delete(int buildingId)
        {
            
        }
    }
}