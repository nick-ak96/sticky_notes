using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetPublicNotesAsync(string filter, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetUserNotesAsync(long userId, string filter, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetSharedWithUserNotesAsync(long userId, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetOrgNotesAsync(long organizationId, CancellationToken cancellationToken);

        Task<long> CreateNoteAsync(Note note, CancellationToken cancellationToken);

        Task<Note> GetNoteAsync(long noteId, CancellationToken cancellationToken);

        Task<Note> GetUserNoteAsync(long userId, long noteId, CancellationToken cancellationToken);

        Task<int> DelteNoteAsync(long noteId, CancellationToken cancellationToken);

        Task<int> UpdateNoteAsync(Note note, CancellationToken cancellationToken);
    }
}
