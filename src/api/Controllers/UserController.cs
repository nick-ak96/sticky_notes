using System.Threading;
using System.Threading.Tasks;
using api.Controllers.Models;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(userId, cancellationToken);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] long userId, [FromBody]User payload, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(userId, cancellationToken);
            if (user == null)
                return NotFound();
            await _userService.UpdateUserAsync(payload, cancellationToken);
            var updatedUser = await _userService.GetUserAsync(userId, cancellationToken);
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

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] long userId, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(userId, cancellationToken);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]UserDetails userDetails, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(userDetails, cancellationToken);
            return CreatedAtAction(nameof(GetUserAsync), new { userId = user.Id }, user);
        }
    }
}
