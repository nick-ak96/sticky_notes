using System.Threading;
using System.Threading.Tasks;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IOrganizationService _orgService;

        public UserController(IUserService userService, IOrganizationService organizationService)
        {
            _userService = userService;
            _orgService = organizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAsync(CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(CurrentUserId, cancellationToken);
            return Ok(user);
        }

        [HttpGet("orgs")]
        public async Task<IActionResult> GetUserOrganizationsAsync(CancellationToken cancellationToken)
        {
            var organizations = await _orgService.GetUserOrganizationsAsync(CurrentUserId, cancellationToken);
            return Ok(organizations);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody]UserUpdate patch, CancellationToken cancellationToken)
        {
            var updatedUser = await _userService.UpdateUserAsync(CurrentUserId, patch, cancellationToken);
            return Ok(updatedUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]UserCredentials credentials, CancellationToken cancellationToken)
        {
            var token = await _userService.Login(credentials, cancellationToken);
            if (token == null)
                return Unauthorized();
            return Ok(new { token });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(CurrentUserId, cancellationToken);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]UserCreate userCreate, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(userCreate, cancellationToken);
            return CreatedAtAction(nameof(GetUserAsync), user);
        }
    }
}
