﻿using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos.User
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
