namespace WebApiTIenda.Application.DTOs.Admin
{
    public class SupplierProductDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string LotCode { get; set; } = null!;
        public DateTime LotExpDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
