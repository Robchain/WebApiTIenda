using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Domian.Entities
{
    public class Product
    {
        
        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
        public ICollection<WishListDetail> WishListDetails { get; set; } = new List<WishListDetail>();
        public ICollection<OrderRDetail> OrderRDetails { get; set; } = new List<OrderRDetail>();
    }
}
