namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class OrderItemSummaryDto
    {
        public string ProductName { get; set; } = null!;
        public string ProductImageUrl { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
