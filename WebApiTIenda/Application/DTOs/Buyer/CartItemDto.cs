namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class CartItemDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductImageUrl { get; set; } = null!;
        public int IdSupplier { get; set; }
        public string SupplierName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
        public int AvailableStock { get; set; }
        public bool IsValidItem => Quantity <= AvailableStock;
    }
}
