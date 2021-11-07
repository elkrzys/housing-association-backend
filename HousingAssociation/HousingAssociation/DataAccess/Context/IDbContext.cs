using System.Data;

namespace DataAccess.Context
{
    public interface IDbContext
    {
        IDbConnection Connection { get; }
    }
}
