using System.Collections.Generic;
using Dapper;
using DataAccess.Context;
using HousingAssociation.Models;

namespace HousingAssociation.Repositories
{
    public class LocalsRepository
    {
        private readonly IDbContext _dbContext;
        public LocalsRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Local> FindAll()
        {
            return new List<Local>();
        }

        public IEnumerable<Local> FindAllByBuildingId(int buildingId)
        {
            var statement = "select * from sysadm.locals where building_id = @buildingId";
            
            using var conn = _dbContext.Connection;
            return conn.Query<Local>(statement, new { buildingId });
        }

        public Local FindById(int id)
        {
            var statement = "select * from sysadm.locals where id = @id";
            
            using var conn = _dbContext.Connection;
            return conn.QuerySingleOrDefault<Local>(statement, new { id });
        }

        public int Add(Local local)
        {
            var statement = @"insert into sysadm.locals (number, area, building_id, is_fully_owned) 
                                values (@Number, @Area, @BuildingId, @IsFullyOwned) returning id";

            var parameters = new
            {
                local.Number,
                local.Area,
                local.BuildingId,
                local.IsFullyOwned
            };
            
            using var conn = _dbContext.Connection;
            return conn.ExecuteScalar<int>(statement, parameters);
        }

        public void Update(Local local)
        {
            var statement = @"update sysadm.locals set number = @Number, area = @Area, building_id = @BuildingId, is_fully_owned = @IsFullyOwned
                                where id = @Id";

            var parameters = new
            {
                local.Id,
                local.Number,
                local.Area,
                local.BuildingId,
                local.IsFullyOwned
            };
            
            using var conn = _dbContext.Connection;
            conn.Execute(statement, parameters);
        }

        public void Delete(int id)
        {
            
        }
    }
}