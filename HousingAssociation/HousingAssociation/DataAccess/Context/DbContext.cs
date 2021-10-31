using System.Data;
using DataAccess.Context;
using Npgsql;

namespace HousingAssociation.DataAccess.Context
{
    public class DbContext : IDbContext
    {
        private readonly IDbConnection _connection;
        public IDbConnection Connection
        {
            get
            {
                if (_connection.State is ConnectionState.Closed)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public DbContext(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
