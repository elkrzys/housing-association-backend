using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using HousingAssociation.Utils.Extensions;
using NUnit.Framework.Internal;

namespace HousingAssociationTests
{
    public class BuildingsServiceTests
    {
        private BuildingsService _buildingsService;
        private DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "HSDb")
            .Options;
        
        [SetUp]
        public void Setup()
        {
            _buildingsService = new BuildingsService(new UnitOfWork(new AppDbContext(_dbContextOptions)));
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

            var expectedBuilding1 = await _buildingsService.AddBuildingWithAddress(building1.AsDto());
            var expectedBuilding2 = await _buildingsService.AddBuildingWithAddress(building2.AsDto());
            var expectedBuilding3 = await _buildingsService.AddBuildingWithAddress(building3.AsDto());
            
            // when
            var expectedBuildings = new List<Building>{expectedBuilding1, expectedBuilding2};
            var searchAddress = new Address {Street = "Poziomkowa"};
            var actualBuildings = await _buildingsService.GetAllBuildingsByAddress(searchAddress);

            // then
            Assert.AreEqual(expectedBuildings.Count, actualBuildings.Count);
            Assert.Contains(expectedBuilding1, actualBuildings);
            Assert.Contains(expectedBuilding2, actualBuildings);
            Assert.Pass();
        }
    }
}