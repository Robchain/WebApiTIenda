namespace WebApiTIenda.Domian.Entities
{
    public class OrderR
    {
        
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public string State { get; set; } = null!;
        public decimal Total { get; set; }

        public ICollection<OrderRDetail> OrderRDetails { get; set; } = new List<OrderRDetail>();
    }
}
