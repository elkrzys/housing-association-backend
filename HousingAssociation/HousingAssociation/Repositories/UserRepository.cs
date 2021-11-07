using System.Collections.Generic;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class UserRepository
    {
        public UserRepository()
        {
                
        }

        public List<User> FindAll()
        {
            return new List<User>();
        }

        public int Add(User user)
        {
            int id = 0;
            return id;
        }

        public void Update(User user)
        {
            
        }

        public void Delete(int userId)
        {
            
        }
    }
}