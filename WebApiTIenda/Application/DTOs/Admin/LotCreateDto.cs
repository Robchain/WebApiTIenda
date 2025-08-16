using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Admin
{
    public class LotCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CodeLote { get; set; } = null!;

        [Required]
        public DateTime DateOfAdmission { get; set; }

        [Required]
        public DateTime ExpDate { get; set; }
    }
}
