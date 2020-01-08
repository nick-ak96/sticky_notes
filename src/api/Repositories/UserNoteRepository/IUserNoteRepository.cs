using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IUserNoteRepository
    {
        Task<NoteAccessType?> GetUserNoteConnectionAsync(long userId, long noteId, CancellationToken cancellationToken);

        Task<IEnumerable<UserNoteSharing>> GetNoteSharedUsersAsync(long noteId, CancellationToken cancellationToken);

        Task<int> CreateUserNoteConnectionAsync(long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task<int> DeleteUserNoteConnectionAsync(long noteId, long userId, CancellationToken cancellationToken);

        Task<int> UpdateUserNoteConnectionAsync(long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken);
    }
}
