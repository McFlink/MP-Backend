using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP_Backend.Helpers;
using MP_Backend.Models.DTOs.Users;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Controllers
{
    [Authorize(Roles = Roles.AllUsers)] // not tested in swagger yet
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDTO>> GetUserInfo(CancellationToken ct)
        {
            var profile = await _userService.GetUserProfileAsync(ct);
            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDTO dto, CancellationToken ct)
        {
            var updatedProfile = await _userService.UpdateProfileAsync(dto, ct);
            return Ok(updatedProfile);
        }

        [HttpPut("profile/email")]
        public async Task<IActionResult> UpdateUserEmail([FromQuery] string newEmail, CancellationToken ct)
        {
            var updatedEmail = await _userService.UpdateEmailAsync(newEmail, ct);
            return Ok(updatedEmail);
        }
    }
}
