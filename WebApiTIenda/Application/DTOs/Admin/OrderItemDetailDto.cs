namespace WebApiTIenda.Application.DTOs.Admin
{
    public class OrderItemDetailDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string LotCode { get; set; } = null!;
    }
}
