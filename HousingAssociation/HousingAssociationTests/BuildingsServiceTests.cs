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
    public class BuildingsServiceTests
    {
        private BuildingsRepository _buildings;
        private BuildingsService _buildingsService;
        private AppDbContext _dbContext;
        private DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "HSDb", new InMemoryDatabaseRoot())
            .Options;
        
        [SetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _buildings = new BuildingsRepository(_dbContext);
            _buildingsService = new BuildingsService(new UnitOfWork(_dbContext));
        }
        [TearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task ShouldGetAllBuildingsByAddress()
        {
            // given
            Address address1 = new Address
            {
                City = "Tychy",
                District = "Paprocany",
                Street = "Poziomkowa"
            };
            Address address2 = new Address
            {
                City = "Tychy",
                District = "Paprocany",
                Street = "Malinowa"
            };
            
            Building building1 = new Building
            {
                Type = BuildingType.Block,
                Number = "1",
                Address = address1
            };
            Building building2 = new Building
            {
                Type = BuildingType.Block,
                Number = "2",
                Address = address1
            };
            Building building3 = new Building
            {
                Type = BuildingType.Block,
                Number = "1",
                Address = address2
            };

            var id1 = await _buildingsService.AddBuildingWithAddress(building1.AsDto());
            var id2 = await _buildingsService.AddBuildingWithAddress(building2.AsDto());
            var id3 = await _buildingsService.AddBuildingWithAddress(building3.AsDto());

            var expectedBuilding1 = (await _buildings.FindByIdAsync(id1)).AsDto();
            var expectedBuilding2 = (await _buildings.FindByIdAsync(id2)).AsDto();
            
            // when
            var expectedBuildings = new List<Building>{building1, building1};
            var searchAddress = new Address {Street = "Poziomkowa"};
            var actualBuildings = await _buildingsService.GetAllBuildingsByAddress(searchAddress);
            
            // then
            Assert.AreEqual(expectedBuildings.Count, actualBuildings.Count);
            Assert.True(actualBuildings.Exists(b => b.Id == expectedBuilding1.Id));
            Assert.True(actualBuildings.Exists(b => b.Id == expectedBuilding2.Id));
            Assert.Pass();
        }

        [Test]
        public async Task ShouldDeleteGivenBuilding()
        {
            Address address = new Address
            {
                City = "Tychy",
                District = "Paprocany",
                Street = "Malinowa"
            };
            Building building = new Building
            {
                Type = BuildingType.Block,
                Number = "1",
                Address = address
            };

            var buildingId = await _buildingsService.AddBuildingWithAddress(building.AsDto());
            var buildingsAfterAdd = await _buildings.FindAllAsync();
            
            Assert.True(buildingsAfterAdd.Any());

            await _buildingsService.DeleteById(buildingId);
            const int expectedCount = 0;
            var buildingsAfterDelete = await _buildings.FindAllAsync();
            var actualCount = buildingsAfterDelete.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

    }
}