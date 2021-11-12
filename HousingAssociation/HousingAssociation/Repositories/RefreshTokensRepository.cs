using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class RefreshTokensRepository
    {
        private readonly DbSet<UserRefreshToken> _tokens;
        
        public RefreshTokensRepository(AppDbContext dbContext)
        {
            _tokens = dbContext.RefreshTokens;
        }

        public async Task Add(UserRefreshToken token)
        {
            
        }
        
        public async Task Remove(UserRefreshToken token)
        {
            
        }
        
        public async Task Refresh(UserRefreshToken token)
        {
            
        }
    }
}