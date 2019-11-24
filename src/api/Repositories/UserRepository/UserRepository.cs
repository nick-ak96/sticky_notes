using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class UserRepository : BaseDBProvider, IUserRepository
    {
        public UserRepository(IDBProvider dbProvider)
            : base(dbProvider)
        {
        }

        public Task<long> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql =
                    @"
                    insert into user (
							username,
							password_hash,
							password_salt,
							name,
							surname,
							insert_date,
							last_modified)
                    values (
							@Username,
							@PasswordHash,
							@PasswordSalt,
							@Name,
							@Surname,
							@InsertDate,
							@LastModified);
					select last_insert_rowid();
                    ";
                con.Open();
                return con.QuerySingleAsync<long>(
                    new CommandDefinition(sql, new
                    {
                        user.Username,
                        user.PasswordHash,
                        user.PasswordSalt,
                        user.Name,
                        user.Surname,
                        user.InsertDate,
                        user.LastModified
                    }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                return Task.FromResult(1);
                // TODO
                var sql = string.Empty;
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DeleteUserAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                return Task.FromResult(1);
                // TODO
                var sql = string.Empty;
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { }, cancellationToken: cancellationToken));
            }
        }

        public Task<User> GetUserAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql =
                    @"
                    select
						id as Id,
                        username as Username,
                        password_hash as PasswordHash,
						password_salt as PasswordSalt,
						name as Name,
						surname as Surname,
						insert_date as InsertDate,
						last_modified as LastModified
                    from
                        user
                    where
                        id = @userId
                    ";
                con.Open();
                return con.QuerySingleOrDefaultAsync<User>(
                    new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken));
            }
        }

        public Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql =
                    @"
                    select
						id as Id,
                        username as Username,
                        password_hash as PasswordHash,
						password_salt as PasswordSalt,
						name as Name,
						surname as Surname,
						insert_date as InsertDate,
						last_modified as LastModified
                    from
                        user
                    where
                        username = @username
                    ";
                con.Open();
                return con.QueryFirstOrDefaultAsync<User>(
                    new CommandDefinition(sql, new { username }, cancellationToken: cancellationToken));
            }
        }
    }
}
