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
    public class LocalsServiceTests
    {
        private UsersRepository _users;
        private BuildingsRepository _buildings;
        private LocalsRepository _locals;
        private LocalsService _localsService;
        private AppDbContext _dbContext;
        private DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "HSDb", new InMemoryDatabaseRoot())
            .Options;

        private async Task PrepareBuildingAndUser()
        {
            await _buildings.AddAsync(new Building
            {
                Number = "1",
                Address = new Address
                {
                    City = "abc", Street = "abc"
                },
                Type = BuildingType.Block
            });
            
            await _users.AddAsync(new User{FirstName = "abc",
                LastName = "abc",
                Email = "user@user.com",
                PhoneNumber = "123456789",
                IsEnabled = true,
                Role = Role.Resident,
                UserCredentials = new UserCredentials{PasswordHash = "1234"}});
            
            await _dbContext.SaveChangesAsync();
        }
        
        [SetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _locals = new LocalsRepository(_dbContext);
            _users = new UsersRepository(_dbContext);
            _buildings = new BuildingsRepository(_dbContext);
            _localsService = new LocalsService(new UnitOfWork(_dbContext));
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task ShouldAddResidentToLocal()
        {
            // given
            await PrepareBuildingAndUser();
            var user = (await _users.FindAllAsync()).First();
            var building = (await _buildings.FindAllAsync()).First();

            var local = new Local
            {
                Area = 25,
                BuildingId = building.Id,
                Number = "1"
            };

            await _locals.AddAsync(local);
            await _dbContext.SaveChangesAsync();
            
            // when

            await _localsService.AddResidentToLocal(local.Id, user.Id);
            var actualLocal = await _locals.FindByIdAsync(local.Id);
            
            // then
            Assert.IsNotEmpty(actualLocal.Residents);
            Assert.AreEqual(actualLocal.Residents.First(), user);
        }
        
        [Test]
        public async Task ShouldRemoveResidentFromLocal()
        {
            // given
            await PrepareBuildingAndUser();
            var user = (await _users.FindAllAsync()).First();
            var building = (await _buildings.FindAllAsync()).First();

            var local = new Local
            {
                Area = 25,
                BuildingId = building.Id,
                Number = "1"
            };

            await _locals.AddAsync(local);
            await _dbContext.SaveChangesAsync();
            await _localsService.AddResidentToLocal(local.Id, user.Id);
           
            // when
            await _localsService.RemoveResidentFromLocal(local.Id, user.Id);
            var actualLocal = await _locals.FindByIdAsync(local.Id);
            
            // then
            Assert.IsEmpty(actualLocal.Residents);
        }
        
       

    }
}