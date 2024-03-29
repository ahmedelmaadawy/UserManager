using System.ComponentModel.DataAnnotations;

namespace User.Manager.API.Models.Authentication.SignUp
{
    public class ResetPasswordModel
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "The Pasword and confirmation password dont match")]
        public string ConfirmPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
