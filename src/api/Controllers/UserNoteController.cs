using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/user/note")]
    [ApiController]
    public class UserNoteController : BaseController
    {
        private readonly INoteService _noteService;

        public UserNoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotesAsync([FromQuery]string filter, CancellationToken cancellationToken)
        {
            var notes = await _noteService.GetUserNotesAsync(CurrentUserId, filter, cancellationToken);
            return Ok(notes);
        }

        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedWithUserNotesAsync(CancellationToken cancellationToken)
        {
            var notes = await _noteService.GetSharedNotesWithUserAsync(CurrentUserId, cancellationToken);
            return Ok(notes);
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetUserNoteAsync(long noteId, CancellationToken cancellationToken)
        {
            var note = await _noteService.GetUserNoteAsync(CurrentUserId, noteId, cancellationToken);
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserNoteAsync([FromBody]NoteCreate note, CancellationToken cancellationToken)
        {
            var newNote = await _noteService.CreateUserNoteAsync(CurrentUserId, note, cancellationToken);
            return CreatedAtAction(nameof(GetUserNoteAsync), new { noteId = newNote.Id }, newNote);
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateUserNoteAsync([FromRoute]long noteId, [FromBody]NoteUpdate note, CancellationToken cancellationToken)
        {
            var updatedNote = await _noteService.UpdateUserNoteAsync(CurrentUserId, noteId, note, cancellationToken);
            return Ok(updatedNote);
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteUserNoteAsync([FromRoute]long noteId, CancellationToken cancellationToken)
        {
            await _noteService.DeleteUserNoteAsync(CurrentUserId, noteId, cancellationToken);
            return NoContent();
        }

		[HttpGet("{noteId}/sharing")]
		public async Task<IActionResult> GetUserNoteSharingInfoAsync([FromRoute]long noteId, CancellationToken cancellationToken)
		{
			var info = await _noteService.GetUserNoteSharingInfoAsync(CurrentUserId, noteId, cancellationToken);
			return Ok(info);
		}
    }
}
