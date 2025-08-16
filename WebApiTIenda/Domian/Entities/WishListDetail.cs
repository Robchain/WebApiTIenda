namespace WebApiTIenda.Domian.Entities
{
    public class WishListDetail
    {
        public int IdList { get; set; }
        public WishList WishList { get; set; } = null!;

        public int IdProduct { get; set; }
        public Product Product { get; set; } = null!;

        public DateTime DateAdded { get; set; }
    }
}
