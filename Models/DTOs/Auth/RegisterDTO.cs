﻿namespace MP_Backend.Models.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
