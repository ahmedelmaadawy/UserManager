using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Manager.API.Models
{
    public class WishListItem
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int WishlistId { get; set; }
        [ForeignKey("WishlistId")]
        public WishList? WishList { get; set; }

    }
}
