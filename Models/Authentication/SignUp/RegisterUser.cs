using System.ComponentModel.DataAnnotations;

namespace User.Manager.API.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "User Name is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string? Password { get; set; }
    }
}
