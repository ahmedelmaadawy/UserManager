using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Manager.API.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public List<WishListItem> WishListItems { get; set; }
    }
}
