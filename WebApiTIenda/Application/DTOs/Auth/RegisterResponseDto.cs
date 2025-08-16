namespace WebApiTIenda.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Message { get; set; } = "Usuario registrado exitosamente";
    }
}
