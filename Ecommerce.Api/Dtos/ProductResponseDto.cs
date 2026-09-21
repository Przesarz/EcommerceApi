namespace Ecommerce.Api.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }   
}
