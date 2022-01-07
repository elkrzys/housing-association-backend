using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Repositories;
using HousingAssociation.Utils.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework.Internal;

namespace HousingAssociationTests
{
    public class UsersServiceTests
    {
        private UsersRepository _users;
        private UsersService _usersService;
        private AppDbContext _dbContext;
        private DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "HSDb", new InMemoryDatabaseRoot())
            .Options;
        
        [SetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _users = new UsersRepository(_dbContext);
            _usersService = new UsersService(new UnitOfWork(_dbContext), null);
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task ShouldDisableUser()
        {
            //given
            var resident = new User
            {
                FirstName = "abc",
                LastName = "abc",
                Email = "user@user.com",
                PhoneNumber = "123456789",
                IsEnabled = true,
                Role = Role.Resident,
                UserCredentials = new UserCredentials{PasswordHash = "1234"}
            };

            await _users.AddAsync(resident);
            await _dbContext.SaveChangesAsync();
            
            // when
            await _usersService.DisableUser(resident.Id, removeCredentials: true);
            var disabledUser = await _users.FindByIdAsync(resident.Id);
            
            //then
            Assert.False(disabledUser.IsEnabled);
            Assert.Null(disabledUser.UserCredentials);
        }
        
        [Test]
        public async Task ShouldUpdateUser()
        {
            //given
            const int expectedUsersCount = 1;
            var user = new User
            {
                FirstName = "abc",
                LastName = "abc",
                Email = "user@user.com",
                PhoneNumber = "123456789",
                IsEnabled = true,
            };

            await _users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var expectedUpdatedUser = user with {FirstName = "user", LastName = "user"};
            
            // when
            int usersCount = (await _users.FindAllAsync()).Count;
            await _usersService.Update(expectedUpdatedUser.AsDto());

            // then
            var actualUpdatedUser = await _users.FindByIdAsync(user.Id);
            Assert.AreEqual(expectedUsersCount, usersCount);
            Assert.AreEqual(expectedUpdatedUser.FirstName, actualUpdatedUser.FirstName);
            Assert.AreEqual(expectedUpdatedUser.LastName, actualUpdatedUser.LastName);
        }
        
        [Test]
        public async Task ShouldBanUser()
        {
            //given
            var resident = new User
            {
                FirstName = "abc",
                LastName = "abc",
                Email = "user@user.com",
                PhoneNumber = "123456789",
                IsEnabled = true,
                Role = Role.Resident,
                UserCredentials = new UserCredentials{PasswordHash = "1234"}
            };

            await _users.AddAsync(resident);
            await _dbContext.SaveChangesAsync();
            
            // when
            await _usersService.DisableUser(resident.Id);
            var disabledUser = await _users.FindByIdAsync(resident.Id);
            Assert.False(disabledUser.IsEnabled);
            Assert.NotNull(disabledUser.UserCredentials);
        }

    }
}