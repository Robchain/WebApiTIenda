namespace WebApiTIenda.Application.DTOs.Admin
{
    public class LotProductDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
