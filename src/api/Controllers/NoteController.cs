using System.Threading;
using System.Threading.Tasks;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPublicNotesAsync([FromQuery]string filter, CancellationToken cancellationToken)
        {
            var notes = await _noteService.GetPublicNotesAsync(filter, cancellationToken);
            return Ok(notes);
        }
    }
}
