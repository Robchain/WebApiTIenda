using System.ComponentModel.DataAnnotations;
using WebApiTIenda.Domian.Enums;

namespace WebApiTIenda.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol es requerido")]
        public UserRoles Rol { get; set; } = UserRoles.Comprador;
    }
}
