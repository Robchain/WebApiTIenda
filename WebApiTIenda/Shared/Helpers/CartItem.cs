namespace WebApiTIenda.Shared.Helpers
{
    public class CartItem
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public int AvailableStock { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
