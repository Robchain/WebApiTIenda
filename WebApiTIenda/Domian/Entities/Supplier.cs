namespace WebApiTIenda.Domian.Entities
{
    public class Supplier
    {
        public int IdSuPlier { get; set; }
        public string Name { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;

        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
    }
}
