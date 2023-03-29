﻿using System.ComponentModel.DataAnnotations;

namespace HD_Backend.Data.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]

        public string? Password { get; set; }

    }
}
