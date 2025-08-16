using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Admin
{
    public class OrderUpdateStateDto
    {
        [Required]
        [StringLength(50)]
        public string State { get; set; } = null!;
    }
}
