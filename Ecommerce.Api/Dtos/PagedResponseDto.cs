namespace Ecommerce.Api.Dtos
{
    public class PagedResponseDto<T>
    {
        public List<T> Items { get; set; } = new();
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required int TotalItems { get; set; }
        public required int TotalPages { get; set; }
    }
}
