using Microsoft.AspNetCore.Identity;

namespace User.Manager.API.Models
{
    public class Applicationuser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
