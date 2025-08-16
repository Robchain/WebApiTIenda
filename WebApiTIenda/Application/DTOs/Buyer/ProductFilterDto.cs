using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class ProductFilterDto : PaginationDto
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; } // "price_asc", "price_desc", "name", "newest"
    }
}
