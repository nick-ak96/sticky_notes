using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class NoteRepository : BaseRepository, INoteRepository
    {
        public NoteRepository(IDBProvider dbProvider) : base(dbProvider)
        {
        }

        public Task<long> CreateNoteAsync(Note note, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					insert into note
						(content, language, color, created_by, insert_date, last_modified)
					values
						(@Content, @Language, @Color, @CreatedBy, @InsertDate, @LastModified);
					select last_insert_rowid()
					";
                con.Open();
                return con.QuerySingleAsync<long>(new CommandDefinition(sql, note, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DelteNoteAsync(long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					delete from note where id = @noteId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId }, cancellationToken: cancellationToken));
            }
        }

        public Task<Note> GetNoteAsync(long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						id as Id,
						content as Content,
						created_by as CreatedBy,
						last_modified as LastModified,
						insert_date as InsertDate,
						color as Color,
						is_pinned as IsPinned,
						is_public as IsPublic,
						language as Language
					from
						note
					where
						id = @noteId
					";
                con.Open();
                return con.QueryFirstOrDefaultAsync<Note>(new CommandDefinition(sql, new { noteId }, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<Note>> GetPublicNotesAsync(string filter, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                dynamic parameters = new ExpandoObject();
                parameters.isPublic = 1;
                string sql = @"
					select
						n.id as Id,
						n.content as Content,
						n.created_by as CreatedBy,
						u.username as Username,
						n.last_modified as LastModified,
						n.color as Color,
						n.language as Language,
						n.insert_date as InsertDate
					from
						note n
						inner join user u on u.id = n.created_by
					where
						is_public = :isPublic
					";
                if (!string.IsNullOrEmpty(filter))
                {
                    sql += @"
						and (
								n.content like @filter
								or
								u.username like @filter
								)

						";
                    parameters.filter = $"%{filter}%";

                }
                con.Open();
                return con.QueryAsync<Note>(new CommandDefinition(sql, (object)parameters, cancellationToken: cancellationToken));
            }
        }

        public Task<Note> GetUserNoteAsync(long userId, long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					select
						id as Id,
						content as Content,
						created_by as CreatedBy,
						last_modified as LastModified,
						color as Color,
						language as Language,
						insert_date as InsertDate
					from
						note
					where
						id = @noteId
						and created_by = @userId
					";
                con.Open();
                return con.QueryFirstOrDefaultAsync<Note>(new CommandDefinition(sql, new { userId, noteId }, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<Note>> GetUserNotesAsync(long userId, string filter, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
				dynamic parameters = new ExpandoObject();
				parameters.userId = userId;
                string sql = @"
					select
						n.id as Id,
						n.content as Content,
						n.language as Language,
						n.color as Color,
						n.is_public as IsPublic,
						n.is_pinned as IsPinned,
						n.created_by as CreatedBy,
						u.username as Username,
						n.last_modified as LastModified,
						n.insert_date as InsertDate
					from
						note n
						inner join user u on n.created_by = u.id
					where
						created_by = @userId
					";
				if (!string.IsNullOrEmpty(filter))
				{
					sql += @"
						and content like @filter
						";
					parameters.filter = $"%{filter}%";
				}
                con.Open();
                return con.QueryAsync<Note>(new CommandDefinition(sql, (object)parameters, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<Note>> GetSharedWithUserNotesAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						n.id as Id,
						n.content as Content,
						n.language as Language,
						n.color as Color,
						n.is_public as IsPublic,
						n.is_pinned as IsPinned,
						n.created_by as CreatedBy,
						u.username as Username,
						n.last_modified as LastModified,
						n.insert_date as InsertDate
					from
						note n
					inner join
						user_note un on n.id = un.note_id and un.user_id = @userId
					inner join
						user u on n.created_by = u.id
					";
                con.Open();
                return con.QueryAsync<Note>(new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<Note>> GetOrgNotesAsync(long organizationId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						n.id as Id,
						n.content as Content,
						n.language as Language,
						n.color as Color,
						n.is_public as IsPublic,
						n.is_pinned as IsPinned,
						n.created_by as CreatedBy,
						n.last_modified as LastModified,
						n.insert_date as InsertDate
					from
						note n
					inner join
						organization_note on n.id = on.note_id and on.organization_id = @organizationId
					";
                con.Open();
                return con.QueryAsync<Note>(new CommandDefinition(sql, new { organizationId }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateNoteAsync(Note note, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					update note set
						content = @Content,
						last_modified = @LastModified,
						color = @Color,
						language = @Language,
						is_pinned = @IsPinned,
						is_public = @IsPublic
					where
						id = @Id
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, note, cancellationToken: cancellationToken));
            }
        }

    }
}
