namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class OrderSummaryDto
    {
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public string State { get; set; } = null!;
        public decimal Total { get; set; }
        public int TotalItems { get; set; }
        public List<OrderItemSummaryDto> Items { get; set; } = new();
    }
}
