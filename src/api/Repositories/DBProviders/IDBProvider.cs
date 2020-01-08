using System.Data;

namespace api.Repositories
{
    public interface IDBProvider
    {
        IDbConnection CreateConnection();
    }
}
