namespace WebApiTIenda.Application.DTOs.Admin
{
    public class OrderDetailDto
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string State { get; set; } = null!;
        public decimal Total { get; set; }
        public List<OrderItemDetailDto> Items { get; set; } = new();
    }
}
