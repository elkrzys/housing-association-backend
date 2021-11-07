using System;
using System.Collections.Generic;
using Dapper;
using DataAccess.Context;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class BuildingsRepository
    {
        private readonly IDbContext _dbContext;
        public BuildingsRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Building> FindAll()
        {
            return new List<Building>();
        }

        public Building FindById(int id)
        {
            var statement = "select * from sysadm.buildings where id = @id";
            
            using var conn = _dbContext.Connection;
            return conn.QuerySingleOrDefault<Building>(statement, new { id });
        }

        public int Add(Building building)
        {
            var statement = @"insert into sysadm.buildings (city, street, district, number, type) 
                                values (@City, @Street, @District, @Number, @Type) returning id";

            var parameters = new
            {
                building.Address.City,
                building.Address.Street,
                building.Address.District,
                building.Number,
                Type = Enum.GetName(building.Type)
            };
            
            using var conn = _dbContext.Connection;
            return conn.ExecuteScalar<int>(statement, parameters);
        }

        public void Update(Building building)
        {
            var statement = @"update sysadm.buildings set city = @City, street = @Street, district = @District, number = @Number, type = @Type
                                where id = @Id";
            
            var parameters = new
            {
                building.Id,
                building.Address.City,
                building.Address.Street,
                building.Address.District,
                building.Number,
                Type = Enum.GetName(building.Type)
            };
            
            using var conn = _dbContext.Connection;
            conn.Execute(statement, parameters);
        }

        public void Delete(int buildingId)
        {
            var statement = "delete from sysadm.buildings where id = @buildingId";
            
            using var conn = _dbContext.Connection;
            conn.Execute(statement, new { buildingId });
        }
    }
}