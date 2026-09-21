using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Dtos
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
