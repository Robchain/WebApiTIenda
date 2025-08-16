namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductListDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!; // ⬅️ AGREGADA
        public string Category { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalStock { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool HasActiveSuppliers { get; set; }
    }
}
