using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.Repositories
{
    public class UsersRepository
    {
        private readonly DbSet<User> _users;
        public UsersRepository(AppDbContext dbContext)
        {
            _users = dbContext.Users;
        }
        public async Task<User> FindByIdAsync(int id) => await _users.FindAsync(id);
        public async Task<User> FindByUserEmailAsync(string email) 
            => await _users
                .Include(u => u.RefreshTokens)
                .Include(u => u.UserCredentials)
                .SingleOrDefaultAsync(u => u.Email.Equals(email));
        public async Task<List<User>> FindByRoleAsync(Role role)
            => await _users.Where(user => user.Role == role).ToListAsync();
        public async Task AddAsync(User user) => await _users.AddAsync(user);
        public void Update(User user) => _users.Update(user);
        public void Delete(User user) => _users.Remove(user);
        public async Task<User> FindByIdAndIncludeAllLocalsAsync(int id)
            => await _users
                .Include(u => u.ResidedLocals)
                .SingleOrDefaultAsync(u => u.Id == id);

        public async Task<List<User>> FindAllAsync() => await _users.ToListAsync();

        public async Task<User> FindByRefreshTokenAsync(string token) 
            => await _users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    }
}