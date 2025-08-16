using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Domian.Entities
{
    public class Lot
    {

        public int IdLot { get; set; }
        public string CodeLote { get; set; } = null!;
        public DateTime DateOfAdmission { get; set; }
        public DateTime ExpDate { get; set; }

        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
    }
}
