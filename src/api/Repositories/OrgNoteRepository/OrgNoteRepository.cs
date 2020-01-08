using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class OrgNoteRepository : BaseRepository, IOrgNoteRepository
    {
        public OrgNoteRepository(IDBProvider dbProvider) : base(dbProvider)
        {
        }

        public Task<int> CreateOrgNoteConnectionAsync(long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					insert into organization_note
						(note_id, organization_id, access_type)
					values
						(@noteId, @organizationId, @accessType)
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, organizationId, accessType }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DeleteOrgNoteConnectionAsync(long noteId, long organizationId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					delete from organization_note
					where
						note_id = @noteId and organization_id = @organizationId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, organizationId }, cancellationToken: cancellationToken));
            }
        }

        public Task<IEnumerable<OrgNoteSharing>> GetNoteSharedOrgsAsync(long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						orgn.organization_id as OrganizationId,
						orgn.note_id as NoteId,
						orgn.access_type as AccessType,
						o.name as Name,
						o.profile_picture as ProfilePicture
					from
						organization_note orgn
						inner join
							organization o on o.id = orgn.organization_id
					where
						orgn.note_id = @noteId
					";
                con.Open();
                return con.QueryAsync<OrgNoteSharing, Organization, OrgNoteSharing>(new CommandDefinition(sql, new { noteId }, cancellationToken: cancellationToken),
                        map: (ons, o) =>
                        {
                            ons.OrganizationDetails = o;
                            return ons;
                        },
                        splitOn: "AccessType,Name");
            }
        }

        public Task<NoteAccessType?> GetOrgNoteConnectionAsync(long organizationId, long noteId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					select
						access_type
					from
						organization_note
					where
						note_id = @noteId and organization_id = @organizationId
					";
                con.Open();
                return con.QueryFirstOrDefaultAsync<NoteAccessType?>(new CommandDefinition(sql, new { noteId, organizationId }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateOrgNoteConnectionAsync(long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql = @"
					update organization_note set
						access_type = @accessType
					where
						note_id = @noteId and organization_id = @organizationId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { noteId, organizationId, accessType }, cancellationToken: cancellationToken));
            }
        }
    }
}
