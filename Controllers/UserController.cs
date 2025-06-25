using Microsoft.AspNetCore.Mvc;
using MP_Backend.Models.DTOs.Users;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDTO dto, CancellationToken ct)
        {
            var updatedProfile = await _userService.UpdateProfileAsync(dto, ct);
            return Ok(updatedProfile);
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateUserEmail([FromQuery] string newEmail, CancellationToken ct)
        {
            var updatedEmail = await _userService.UpdateEmailAsync(newEmail, ct);
            return Ok(updatedEmail);
        }
    }
}
