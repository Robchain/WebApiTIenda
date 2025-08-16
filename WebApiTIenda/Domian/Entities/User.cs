using WebApiTIenda.Domian.Enums;

namespace WebApiTIenda.Domian.Entities
{
    public class User
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRoles Rol { get; set; } = UserRoles.Comprador;
        public DateTime DateCreated { get; set; }

        public ICollection<WishList> WishLists { get; set; } = new List<WishList>();
        public ICollection<OrderR> OrderRs { get; set; } = new List<OrderR>();
    }
}
