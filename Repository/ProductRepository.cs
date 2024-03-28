using Microsoft.EntityFrameworkCore;
using User.Manager.API.Models;

namespace User.Manager.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product is not null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var result = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            return result;
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            var oldproduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (oldproduct is not null)
            {
                oldproduct.Name = product.Name;
                oldproduct.Description = product.Description;
                oldproduct.Price = product.Price;
                await _context.SaveChangesAsync();
            }

        }
    }
}
