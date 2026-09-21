using System.Text.Json.Serialization;

namespace Ecommerce.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; } = new();
    }
}
