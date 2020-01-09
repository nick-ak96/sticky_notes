using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDBProvider dbProvider) : base(dbProvider)
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
							language,
							profile_picture,
							insert_date,
							last_modified)
                    values (
							@Username,
							@PasswordHash,
							@PasswordSalt,
							@Name,
							@Surname,
							@Language,
							@ProfilePicture,
							@InsertDate,
							@LastModified);
					select last_insert_rowid();
                    ";
                con.Open();
                return con.QuerySingleAsync<long>(
                    new CommandDefinition(sql,
                        new
                        {
                            user.Username,
                            user.PasswordHash,
                            user.PasswordSalt,
                            user.Name,
                            user.Surname,
                            user.Language,
                            user.ProfilePicture,
                            user.InsertDate,
                            user.LastModified
                        }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					update
						user
					set
						name = @Name,
						surname = @Surname,
						language = @Language,
						profile_picture = @ProfilePicture,
						last_modified = @LastModified,
						password_hash = @PasswordHash,
						password_salt = @PasswordSalt
					where
						id = @Id
					";
                con.Open();
                return con.ExecuteAsync(
                    new CommandDefinition(sql,
                        new
                        {
                            user.Name,
                            user.Surname,
                            user.Language,
                            user.ProfilePicture,
                            user.LastModified,
							user.PasswordHash,
							user.PasswordSalt,
                            user.Id
                        }, cancellationToken: cancellationToken));
            }
        }
        public Task<int> UpdateUserAsync(UserResponse user, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					update
						user
					set
						name = @Name,
						surname = @Surname,
						language = @Language,
						profile_picture = @ProfilePicture,
						last_modified = @LastModified
					where
						id = @Id
					";
                con.Open();
                return con.ExecuteAsync(
                    new CommandDefinition(sql,
                        new
                        {
                            user.Name,
                            user.Surname,
                            user.Language,
                            user.ProfilePicture,
                            user.LastModified,
                            user.Id
                        }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DeleteUserAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = "delete from user where id = @userId";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken));
            }
        }

        public Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql =
                    @"
                    select
						id as Id,
                        username as Username,
						name as Name,
						surname as Surname,
						language as Language,
						profile_picture as ProfilePicture,
						insert_date as InsertDate,
						last_modified as LastModified
                    from
						user
					where
						id = @userId
                    ";
                con.Open();
                return con.QuerySingleOrDefaultAsync<UserResponse>(
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
						language as Language,
						profile_picture as ProfilePicture,
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
