using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class RefreshTokensRepository
    {
        private readonly DbSet<RefreshToken> _tokens;
        
        public RefreshTokensRepository(AppDbContext dbContext)
        {
            _tokens = dbContext.RefreshTokens;
        }

        public async Task Add(RefreshToken token)
        {
            
        }
        
        public async Task Remove(RefreshToken token)
        {
            
        }
        
        public async Task Refresh(RefreshToken token)
        {
            
        }
    }
}