using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/org/{organizationId}/user/{userId}")]
    [ApiController]
    public class OrganizationUserController : BaseController
    {
        private readonly IOrganizationService _orgService;

        public OrganizationUserController(IOrganizationService organizationService)
        {
            _orgService = organizationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToOrganizationAsync([FromRoute]long organizationId, [FromRoute]long userId,
                [FromQuery]OrganizationAccessType accessType, CancellationToken cancellationToken)
        {
            await _orgService.AddUserToOrganizationAsync(CurrentUserId, organizationId, userId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserOrganizationAccessAsync([FromRoute]long organizationId, [FromRoute]long userId,
                [FromQuery]OrganizationAccessType accessType, CancellationToken cancellationToken)
        {
            await _orgService.UpdateUserOrganizationAccessAsync(CurrentUserId, organizationId, userId, accessType, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromOrganization([FromRoute]long organizationId, [FromRoute]long userId, CancellationToken cancellationToken)
        {
            await _orgService.RemoveUserFromOrganizationAsync(CurrentUserId, organizationId, userId, cancellationToken);
            return NoContent();
        }
    }
}
