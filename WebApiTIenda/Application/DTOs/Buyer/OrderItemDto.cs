using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class OrderItemDto
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdSupplier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
