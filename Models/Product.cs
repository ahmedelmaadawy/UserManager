using System.ComponentModel.DataAnnotations;

namespace User.Manager.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Product Description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Product Price is required")]
        public int? Price { get; set; }
    }

}
