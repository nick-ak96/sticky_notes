using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/user/note/{noteId}/share")]
    [ApiController]
    public class NoteSharingController : BaseController
    {
        private readonly INoteService _noteService;

        public NoteSharingController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost("public")]
        public async Task<IActionResult> ShareNoteWithPublicAsync([FromRoute]long noteId, CancellationToken cancellationToken)
        {
            await _noteService.ShareNoteWithPublicAsync(CurrentUserId, noteId, cancellationToken);
            return NoContent();
        }

        [HttpDelete("public")]
        public async Task<IActionResult> WithholdNoteFromPublicAsync([FromRoute]long noteId, CancellationToken cancellationToken)
        {
            await _noteService.WithholdNoteFromPublicAsync(CurrentUserId, noteId, cancellationToken);
            return NoContent();
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> ShareNoteWithUserAsync([FromRoute]long noteId, [FromRoute]long userId,
                [FromQuery]NoteAccessType accessType, CancellationToken cancellationToken)
        {
            await _noteService.ShareNoteWithUserAsync(CurrentUserId, noteId, userId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateSharingWithUserAsync([FromRoute]long noteId, [FromRoute]long userId,
                [FromQuery]NoteAccessType accessType, CancellationToken cancellationToken)
        {
            await _noteService.UpdateUserNoteConnectionAsync(CurrentUserId, noteId, userId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> WithholdNoteFromUserAsync([FromRoute]long noteId, [FromRoute]long userId, CancellationToken cancellationToken)
        {
            await _noteService.WithholdNoteFromUserAsync(CurrentUserId, noteId, userId, cancellationToken);
            return NoContent();
        }

        [HttpPost("org/{organizationId}")]
        public async Task<IActionResult> ShareNoteWithOrgAsync([FromRoute]long noteId, [FromRoute]long organizationId,
                [FromQuery]NoteAccessType accessType, CancellationToken cancellationToken)
        {
            await _noteService.ShareNoteWithOrgAsync(CurrentUserId, noteId, organizationId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpPut("org/{organizationId}")]
        public async Task<IActionResult> UpdateSharingWithOrgAsync([FromRoute]long noteId, [FromRoute]long organizationId,
                [FromQuery]NoteAccessType accessType, CancellationToken cancellationToken)
        {
            await _noteService.UpdateOrgNoteConnectionAsync(CurrentUserId, noteId, organizationId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpDelete("org/{organizationId}")]
        public async Task<IActionResult> WithholdNoteFromOrgAsync([FromRoute]long noteId, [FromRoute]long organizationId, CancellationToken cancellationToken)
        {
            await _noteService.WithholdNoteFromOrgAsync(CurrentUserId, noteId, organizationId, cancellationToken);
            return NoContent();
        }
    }
}
