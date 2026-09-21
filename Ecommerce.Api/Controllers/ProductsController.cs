using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _context.Products.Include(p=>p.Category).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            var newProduct = product; // dto later

            newProduct.Category = null!;

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product, int id)
        {
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.StockQuantity = product.StockQuantity;
            productToUpdate.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(productToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToDelete == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
