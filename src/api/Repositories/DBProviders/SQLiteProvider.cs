using System.Data;
using api.Models.Options;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace api.Repositories
{
    public class SQLiteProvider : IDBProvider
    {
        private readonly DbContext _dbContext;

        public SQLiteProvider(IOptions<DbContext> options)
        {
            _dbContext = options.Value;
        }

        public IDbConnection CreateConnection() =>
            new SqliteConnection(_dbContext.ConnectionString);
    }
}
