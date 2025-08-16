namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductDetailDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public List<ProductSupplierDetailDto> Suppliers { get; set; } = new();
    }
}
