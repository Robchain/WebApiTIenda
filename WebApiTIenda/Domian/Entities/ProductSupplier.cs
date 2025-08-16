namespace WebApiTIenda.Domian.Entities
{
    public class ProductSupplier
    {
        public int IdProduct { get; set; }
        public Product Product { get; set; } = null!;

        public int IdSupplier { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public int IdLot { get; set; }
        public Lot Lot { get; set; } = null!;

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
