using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class UpdateCartItemDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }
}
