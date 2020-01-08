using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class UserNoteRepository : BaseRepository, IUserNoteRepository
    {
        public UserNoteRepository(IDBProvider dbProvider) : base(dbProvider)
        {
        }

        public Task<int> CreateUserNoteConnectionAsync(long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					insert into user_note
						(note_id, user_id, access_type)
					values
						(@noteId, @userId, @accessType)
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, userId, accessType }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DeleteUserNoteConnectionAsync(long noteId, long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					delete from user_note where note_id = @noteId and user_id = @userId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, userId }, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<UserNoteSharing>> GetNoteSharedUsersAsync(long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						un.user_id as UserId,
						un.note_id as NoteId,
						un.access_type as AccessType,
						u.language as Language,
						u.username as Username,
						u.name as Name,
						u.surname as Surname,
						profile_picture as ProfilePicture
					from
						user_note un
						inner join
							user u on u.id = un.user_id
					where
						un.note_id = @noteId
					";
                con.Open();
                return con.QueryAsync<UserNoteSharing, User, UserNoteSharing>(new CommandDefinition(sql, new { noteId }, cancellationToken: cancellationToken),
                        map: (uns, u) =>
                        {
                            uns.UserDetails = u;
                            return uns;
                        },
                        splitOn: "AccessType,Language");
            }
        }

        public Task<NoteAccessType?> GetUserNoteConnectionAsync(long userId, long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						note_id as NoteId,
						user_id as UserId,
						access_type as AccessType
					from
						user_note
					where
						user_id = @userId and note_id = @noteId
					";
                con.Open();
                return con.QueryFirstOrDefaultAsync<NoteAccessType?>(new CommandDefinition(sql, new { userId, noteId }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateUserNoteConnectionAsync(long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					update user_note set
						access_type = @accessType
					where
						note_id = @noteId and user_id = @userId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, userId, accessType }, cancellationToken: cancellationToken));
            }
        }
    }
}
