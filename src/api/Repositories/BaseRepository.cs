using System.Data;

namespace api.Repositories
{
    public class BaseRepository
    {
        private readonly IDBProvider _dbProvider;

        public BaseRepository(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public IDbConnection CreateConnection()
        {
            return _dbProvider.CreateConnection();
        }
    }
}
