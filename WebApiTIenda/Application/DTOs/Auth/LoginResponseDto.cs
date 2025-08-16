namespace WebApiTIenda.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
