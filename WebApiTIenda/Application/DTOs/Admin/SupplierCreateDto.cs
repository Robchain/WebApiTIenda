using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Admin
{
    public class SupplierCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Contact { get; set; } = null!;

        [Required]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Phone { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;
    }
}
