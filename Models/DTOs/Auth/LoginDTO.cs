using System.ComponentModel.DataAnnotations;

namespace MP_Backend.Models.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
