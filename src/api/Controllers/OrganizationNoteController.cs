using System.Threading;
using System.Threading.Tasks;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/org/{organizationId}/note")]
    [ApiController]
    public class OrganizationNoteController : BaseController
    {
        private readonly INoteService _noteService;

        public OrganizationNoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrgNotesAsync([FromRoute]long organizationId, CancellationToken cancellationToken)
        {
            var notes = await _noteService.GetOrganizationNotesAsync(organizationId, cancellationToken);
            return Ok(notes);
        }
    }
}
