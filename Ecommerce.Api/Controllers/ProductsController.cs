using Ecommerce.Api.Data;
using Ecommerce.Api.Dtos;
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
        public async Task<ActionResult> GetProducts(string? search, decimal? minPrice,
            decimal? maxPrice, int? categoryId, string? sortBy, string? sortOrder)
        {

            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            if (search != null)
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            switch (sortBy)
            {
                case "price":
                    switch (sortOrder)
                    {
                        case "asc":
                            query = query.OrderBy(p => p.Price);
                            break;
                        case "desc":
                            query = query.OrderByDescending(p => p.Price);
                            break;
                        default:
                            query = query.OrderBy(p => p.Price);
                            break;
                    }
                    break;
                case "name":
                    switch (sortOrder)
                    {
                        case "asc":
                            query = query.OrderBy(p => p.Name);
                            break;
                        case "desc":
                            query = query.OrderByDescending(p => p.Name);
                            break;
                        default:
                            query = query.OrderBy(p => p.Name);
                            break;
                    }
                    break;
                default:
                    break;
            }

            var products = await query.ToListAsync();

            var responseDtos = products.Select(product => new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            });

            return Ok(responseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var responseDto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };

            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest();
            }

            var newProduct = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                CategoryId = productDto.CategoryId,
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity,
                CategoryId = newProduct.CategoryId,
                CategoryName = category.Name
            };

            return CreatedAtAction(nameof(GetProduct), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductDto productDto, int id)
        {
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest();
            }

            productToUpdate.Name = productDto.Name;
            productToUpdate.Description = productDto.Description;
            productToUpdate.Price = productDto.Price;
            productToUpdate.StockQuantity = productDto.StockQuantity;
            productToUpdate.CategoryId = productDto.CategoryId;

            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                Id = productToUpdate.Id,
                Name = productToUpdate.Name,
                Description = productToUpdate.Description,
                Price = productToUpdate.Price,
                StockQuantity = productToUpdate.StockQuantity,
                CategoryId = productToUpdate.CategoryId,
                CategoryName = category.Name
            };

            return Ok(responseDto);
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
