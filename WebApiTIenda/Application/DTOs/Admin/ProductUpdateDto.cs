using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        [Url(ErrorMessage = "La URL de la imagen no es válida")]
        public string ImageUrl { get; set; } = null!;
    }
}
