using System.Collections.Generic;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class LocalsRepository
    {
        public LocalsRepository()
        {
                
        }

        public List<Local> FindAll()
        {
            return new List<Local>();
        }

        public List<Local> FindAllByBuildingId(int buildingId)
        {
            return new List<Local>();
        }

        public Local FindById(int id)
        {
            return new Local();
        }

        public int Add(Local local)
        {
            int id = 0;
            return id;
        }

        public void Update(Local local)
        {
            
        }

        public void Delete(int id)
        {
            
        }
    }
}