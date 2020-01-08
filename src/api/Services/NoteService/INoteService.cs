using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetPublicNotesAsync(string filter, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetUserNotesAsync(long requester, string filter, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetOrganizationNotesAsync(long organizationId, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetSharedNotesWithUserAsync(long requester, CancellationToken cancellationToken);

        //Task<IEnumerable<UserNoteSharing>> GetSharedUsersAsync(long noteId, CancellationToken cancellationToken);

        //Task<IEnumerable<OrgNoteSharing>> GetSharedOrgsAsync(long noteId, CancellationToken cancellationToken);

        Task<Note> GetUserNoteAsync(long requester, long noteId, CancellationToken cancellationToken);

        Task<Note> CreateUserNoteAsync(long requester, NoteCreate note, CancellationToken cancellationToken);

        Task<Note> UpdateUserNoteAsync(long requester, long noteId, NoteUpdate note, CancellationToken cancellationToken);

        Task DeleteUserNoteAsync(long requester, long noteId, CancellationToken cancellationToken);

        Task ShareNoteWithPublicAsync(long requester, long noteId, CancellationToken cancellationToken);

        Task WithholdNoteFromPublicAsync(long requester, long noteId, CancellationToken cancellationToken);

        Task ShareNoteWithUserAsync(long requester, long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task WithholdNoteFromUserAsync(long requester, long noteId, long userId, CancellationToken cancellationToken);

        Task UpdateUserNoteConnectionAsync(long requester, long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task ShareNoteWithOrgAsync(long requester, long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task WithholdNoteFromOrgAsync(long requester, long noteId, long organizationId, CancellationToken cancellationToken);

        Task UpdateOrgNoteConnectionAsync(long requester, long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken);

        Task<NoteSharingInfo> GetUserNoteSharingInfoAsync(long requester, long noteId, CancellationToken cancellationToken);
    }
}
