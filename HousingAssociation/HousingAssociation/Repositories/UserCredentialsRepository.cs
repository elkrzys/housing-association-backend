using System;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class UserCredentialsRepository
    {
        private readonly DbSet<UserCredentials> _credentials;
        
        public UserCredentialsRepository(AppDbContext dbContext)
        {
            _credentials = dbContext.Credentials;
        }

        public async Task Add(UserCredentials credentials)
        {
            var existingUserCredentials = await _credentials.FindAsync(credentials.UserId);

            if (existingUserCredentials is null)
            {
                await _credentials.AddAsync(credentials);
            }
        }

        public async Task<UserCredentials> Update(UserCredentials credentials)
        {
            var existingUserCredentials = await _credentials.FindAsync(credentials.UserId);
            if (existingUserCredentials is null)
            {
                _credentials.Update(credentials);
            }
            return null;
        }
        
        public async Task<UserCredentials> FindByUserId(int userId)
        {
            return await _credentials.FindAsync(userId);
        }
    }
}