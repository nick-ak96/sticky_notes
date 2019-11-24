using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface INoteService
    {
		//Task<Paginated<Note>> SearchPublicNotesAsync(NotesSearchInput filter, CancellationToken cancellationToken);

		Task<Paginated<Note>> GetAllUserNotes(long userId, CancellationToken cancellationToken);

        Task<Note> CreateNoteAsync(Note note, CancellationToken cancellationToken);
        Task<Note> GetNoteAsync(long noteId, CancellationToken cancellationToken);
        Task<Note> UpdateNoteAsync(Note note, CancellationToken cancellationToken);
        Task DeleteNoteAsync(long noteId, CancellationToken cancellationToken);

        Task ShareNoteWithUserAsync(long noteId, long userId, AccessType accessType, CancellationToken cancellationToken);
		Task WithholdNoteFromUserAsync(long noteId, long userId, CancellationToken cancellationToken);
        Task ShareNoteWithOrgAsync(long noteId, long organizationId, AccessType accessType, CancellationToken cancellationToken);
		Task WithholdNoteFromOrgAsync(long noteId, long organizationId, CancellationToken cancellationToken);

        //Task<Paginated<Note>> GetUserNotesAsync(long userId, CancellationToken cancellationToken);
    }
}
