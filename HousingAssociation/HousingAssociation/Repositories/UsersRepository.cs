﻿using System;
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

        public async Task<User> Add(User user)
        {
            var existingUser = await _users.FirstOrDefaultAsync(u =>
                u.FirstName.Equals(user.FirstName) && u.LastName.Equals(user.LastName) && u.Email.Equals(user.Email) &&
                u.PhoneNumber.Equals(user.PhoneNumber) && u.Role.Equals(user.Role));

            if (existingUser is null)
            {
                await _users.AddAsync(user);
                return user;
            }
            return null;
        }

        public async Task<User> Update(User user)
        {
            var existingUser = await _users.FindAsync(user.Id);
            if (existingUser is null)
            {
                _users.Update(user);
            }
            return null;
        }

        public async Task<User> FindById(int id)
        {
            return await _users.FindAsync(id);
        }
    }
}