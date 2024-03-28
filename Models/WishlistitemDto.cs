using System.ComponentModel.DataAnnotations;

namespace User.Manager.API.Models
{
    public class WishlistitemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Amount { get; set; }

    }
}
