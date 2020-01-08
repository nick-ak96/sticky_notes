using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Models.Exceptions;
using api.Repositories;

namespace api.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserService _userService;
        private readonly IUserNoteRepository _userNoteRepository;
        private readonly IOrganizationService _orgService;
        private readonly IOrgNoteRepository _orgNoteRepository;

        public NoteService(INoteRepository noteRepository, IUserService userService,
                IUserNoteRepository userNoteRepository, IOrganizationService orgService,
                IOrgNoteRepository orgNoteRepository)
        {
            _noteRepository = noteRepository;
            _userService = userService;
            _userNoteRepository = userNoteRepository;
            _orgService = orgService;
            _orgNoteRepository = orgNoteRepository;
        }

        public Task<IEnumerable<Note>> GetPublicNotesAsync(string filter, CancellationToken cancellationToken)
        {
            return _noteRepository.GetPublicNotesAsync(filter, cancellationToken);
        }

        public Task<IEnumerable<Note>> GetUserNotesAsync(long requester, string filter, CancellationToken cancellationToken)
        {
            return _noteRepository.GetUserNotesAsync(requester, filter, cancellationToken);
        }

        public Task<IEnumerable<Note>> GetOrganizationNotesAsync(long organizationId, CancellationToken cancellationToken)
        {
            return _noteRepository.GetOrgNotesAsync(organizationId, cancellationToken);
        }

        private async Task<Note> GetNoteAsync(long noteId, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetNoteAsync(noteId, cancellationToken);
            if (note == null)
                throw new NotFoundException($"Note {noteId} was not found");
            return note;
        }

        public async Task<Note> GetUserNoteAsync(long requester, long noteId, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetUserNoteAsync(requester, noteId, cancellationToken);
            if (note == null)
                throw new NotFoundException($"Note {noteId} was not found");
            return note;
        }

        public Task<IEnumerable<Note>> GetSharedNotesWithUserAsync(long requester, CancellationToken cancellationToken)
        {
            return _noteRepository.GetSharedWithUserNotesAsync(requester, cancellationToken);
        }

        //public Task<IEnumerable<UserNoteSharing>> GetSharedUsersAsync(long noteId, CancellationToken cancellationToken)
        //{
        //    return _userNoteRepository.GetNoteSharedUsersAsync(noteId, cancellationToken);
        //}

        //public Task<IEnumerable<OrgNoteSharing>> GetSharedOrgsAsync(long noteId, CancellationToken cancellationToken)
        //{
        //    return _orgNoteRepository.GetNoteSharedOrgsAsync(noteId, cancellationToken);
        //}

        public async Task<Note> CreateUserNoteAsync(long requester, NoteCreate note, CancellationToken cancellationToken)
        {
            var newNote = new Note(note, requester);
            long noteId = await _noteRepository.CreateNoteAsync(newNote, cancellationToken);
            return await GetNoteAsync(noteId, cancellationToken);
        }

        public async Task<Note> UpdateUserNoteAsync(long requester, long noteId, NoteUpdate note, CancellationToken cancellationToken)
        {
            var oldNote = await GetNoteAsync(noteId, cancellationToken);
            if (!oldNote.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            oldNote.Patch(note);
            var result = await _noteRepository.UpdateNoteAsync(oldNote, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating note {noteId}");
            return await GetNoteAsync(noteId, cancellationToken);
        }

        public async Task DeleteUserNoteAsync(long requester, long noteId, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            var result = await _noteRepository.DelteNoteAsync(noteId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error deleting note {noteId}");
        }

        public async Task ShareNoteWithPublicAsync(long requester, long noteId, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            note.IsPublic = true;
            var result = await _noteRepository.UpdateNoteAsync(note, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating note {noteId}");
        }

        public async Task WithholdNoteFromPublicAsync(long requester, long noteId, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            note.IsPublic = false;
            var result = await _noteRepository.UpdateNoteAsync(note, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating note {noteId}");
        }

        public async Task ShareNoteWithUserAsync(long requester, long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken); // check if note exsists
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            if (note.CreatedBy.Equals(userId))
                throw new ConflictException("Cannon share note with yourself");
            var user = await _userService.GetUserAsync(userId, cancellationToken); // check if user exists
            var result = await _userNoteRepository.CreateUserNoteConnectionAsync(noteId, userId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error sharing note {noteId} with user {userId}");
        }

        public async Task WithholdNoteFromUserAsync(long requester, long noteId, long userId, CancellationToken cancellationToken)
        {
            if (requester.Equals(userId))
                throw new ConflictException($"Cannot withhold rights to note {noteId} for user {userId}");
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            await _userService.GetUserAsync(userId, cancellationToken); // check if user exists
            var result = await _userNoteRepository.DeleteUserNoteConnectionAsync(noteId, userId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error withholding note {noteId} from user {userId}");
        }

        public async Task UpdateUserNoteConnectionAsync(long requester, long noteId, long userId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            if (requester.Equals(userId))
                throw new ConflictException("Cannon update sharing options with yourself");
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            await _userService.GetUserAsync(userId, cancellationToken);
            var result = await _userNoteRepository.UpdateUserNoteConnectionAsync(noteId, userId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating sharing info on note {noteId} for user {userId}");
        }

        public async Task ShareNoteWithOrgAsync(long requester, long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            var userOrganizations = await _orgService.GetUserOrganizationsAsync(requester, cancellationToken);
            if (!userOrganizations.Any(x => x.Id.Equals(organizationId)))
                throw new ForbiddenException($"User {requester} does not belong to organization {organizationId}");
            var result = await _orgNoteRepository.CreateOrgNoteConnectionAsync(noteId, organizationId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error sharing note {noteId} with organization {organizationId}");
        }

        public async Task WithholdNoteFromOrgAsync(long requester, long noteId, long organizationId, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            var userOrganizations = await _orgService.GetUserOrganizationsAsync(requester, cancellationToken);
            if (!userOrganizations.Any(x => x.Id.Equals(organizationId)))
                throw new ForbiddenException($"User {requester} does not belong to organization {organizationId}");
            var result = await _orgNoteRepository.DeleteOrgNoteConnectionAsync(noteId, organizationId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error withholding note {noteId} from organization {organizationId}");
        }

        public async Task UpdateOrgNoteConnectionAsync(long requester, long noteId, long organizationId, NoteAccessType accessType, CancellationToken cancellationToken)
        {
            var note = await GetNoteAsync(noteId, cancellationToken);
            if (!note.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");
            var userOrganizations = await _orgService.GetUserOrganizationsAsync(requester, cancellationToken);
            if (!userOrganizations.Any(x => x.Id.Equals(organizationId)))
                throw new ForbiddenException($"User {requester} does not belong to organization {organizationId}");
            var result = await _orgNoteRepository.UpdateOrgNoteConnectionAsync(noteId, organizationId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating sharing info on note {noteId} for organization {organizationId}");
        }

        public async Task<NoteSharingInfo> GetUserNoteSharingInfoAsync(long requester, long noteId, CancellationToken cancellationToken)
        {
			var note = await GetNoteAsync(noteId, cancellationToken);
			if (!note.CreatedBy.Equals(requester))
				throw new ForbiddenException($"User {requester} is not an owner of the note {noteId}");

			var sharingInfo = new NoteSharingInfo(noteId);
			sharingInfo.IsPublic = note.IsPublic;
			sharingInfo.Users = await _userNoteRepository.GetNoteSharedUsersAsync(noteId, cancellationToken);
			sharingInfo.Organizations = await _orgNoteRepository.GetNoteSharedOrgsAsync(noteId, cancellationToken);
			return sharingInfo;
        }
    }
}
