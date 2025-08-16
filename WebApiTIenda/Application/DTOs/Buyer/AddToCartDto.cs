using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class AddToCartDto
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdSupplier { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }
}
