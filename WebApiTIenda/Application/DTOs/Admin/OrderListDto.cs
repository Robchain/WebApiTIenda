namespace WebApiTIenda.Application.DTOs.Admin
{
    public class OrderListDto
    {
        public int IdOrder { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string State { get; set; } = null!;
        public decimal Total { get; set; }
        public int TotalItems { get; set; }
    }
}
