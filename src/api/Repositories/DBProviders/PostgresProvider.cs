using System.Data;
using api.Models.Options;
using Microsoft.Extensions.Options;

namespace api.Repositories
{
    public class PostgresProvider : IDBProvider
    {
        private readonly DbContext _dbContext;

        public PostgresProvider(IOptions<DbContext> options)
        {
            _dbContext = options.Value;
        }

        public IDbConnection CreateConnection()
        {
            // TODO the Posgres nuget is required
            throw new System.NotImplementedException();
        }
    }
}
