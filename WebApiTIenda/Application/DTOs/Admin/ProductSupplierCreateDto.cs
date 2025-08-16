using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductSupplierCreateDto
    {

        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdSupplier { get; set; }

        [Required]
        public int IdLot { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }
}
