using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IOrgNoteRepository
    {
        Task<NoteAccessType?> GetOrgNoteConnectionAsync(long organizationId, long noteId, CancellationToken cancellationToken);

        Task<IEnumerable<OrgNoteSharing>> GetNoteSharedOrgsAsync(long noteId, CancellationToken cancellationToken);

        Task<int> CreateOrgNoteConnectionAsync(long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task<int> DeleteOrgNoteConnectionAsync(long noteId, long organizationId, CancellationToken cancellationToken);

        Task<int> UpdateOrgNoteConnectionAsync(long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken);
    }
}
