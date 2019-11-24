using System.Data;

namespace api.Repositories
{
    public class BaseDBProvider
    {
        private readonly IDBProvider _dbProvider;

        public BaseDBProvider(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public IDbConnection CreateConnection()
        {
            return _dbProvider.CreateConnection();
        }
    }
}
