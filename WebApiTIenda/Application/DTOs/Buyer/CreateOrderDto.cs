using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class CreateOrderDto
    {
        [Required]
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
