using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/org")]
    [ApiController]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService _orgService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _orgService = organizationService;
        }

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetOrganizationAsync([FromRoute]long organizationId, CancellationToken cancellationToken)
        {
            var organization = await _orgService.GetOrganizationAsync(organizationId, cancellationToken);
            return Ok(organization);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganizationAsync([FromBody]OrganizationCreate organization, CancellationToken cancellationToken)
        {
            var newOrganization = await _orgService.CreateOrganizationAsync(CurrentUserId, organization, cancellationToken);
            return CreatedAtAction(nameof(GetOrganizationAsync), new { organizationId = newOrganization.Id }, newOrganization);
        }

        [HttpPut("{organizationId}")]
        public async Task<IActionResult> UpdateOrganizationAsync([FromRoute]long organizationId, [FromBody]OrganizationUpdate organization, CancellationToken cancellationToken)
        {
            var updatedOrganization = await _orgService.UpdateOrganizationAsync(CurrentUserId, organizationId, organization, cancellationToken);
            return Ok(updatedOrganization);
        }

        [HttpDelete("{organizationId}")]
        public async Task<IActionResult> DeleteOrganizationAsync([FromRoute]long organizationId, CancellationToken cancellationToken)
        {
            await _orgService.DeleteOrganizationAsync(CurrentUserId, organizationId, cancellationToken);
            return NoContent();
        }
    }
}
