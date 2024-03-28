using Microsoft.AspNetCore.Mvc;
using User.Manager.API.Models;
using User.Manager.API.Repository;

namespace User.Manager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _repository.GetAllAsync();
            return Ok(allProducts);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product is null)
            {
                return BadRequest("Product IS Null");
            }
            await _repository.CreateProductAsync(product);
            return StatusCode(201);
        }
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _repository.DeleteProductAsync(productId);
            return NoContent();
        }
        [HttpPut("{productId:int}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product product)
        {
            if (product is null)
                return BadRequest("Product object is null");
            await _repository.UpdateProductAsync(productId, product);
            return NoContent();
        }
    }
}
