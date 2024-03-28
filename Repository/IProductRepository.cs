using User.Manager.API.Models;

namespace User.Manager.API.Repository
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);

        Task UpdateProductAsync(int id, Product product);
        Task DeleteProductAsync(int productId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
