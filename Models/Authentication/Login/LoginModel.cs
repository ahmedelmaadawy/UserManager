﻿using System.ComponentModel.DataAnnotations;

namespace User.Manager.API.Models.Authentication.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
