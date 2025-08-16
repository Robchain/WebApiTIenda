using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Domian.Entities
{
    public class OrderRDetail
    {
      
        public int IdOrder { get; set; }
        public OrderR OrderR { get; set; } = null!;

        public int IdProduct { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
