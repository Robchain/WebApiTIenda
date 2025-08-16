namespace WebApiTIenda.Application.DTOs.Auth
{
    public class UserProfileDto
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
