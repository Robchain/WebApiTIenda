namespace WebApiTIenda.Application.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }         // IdUser
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
