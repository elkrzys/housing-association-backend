using System.Data;
using DataAccess.Context;
using Npgsql;

namespace HousingAssociation.DataAccess.Context
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;
        public IDbConnection Connection => GetNewOpenConnection();
        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        private IDbConnection GetNewOpenConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
