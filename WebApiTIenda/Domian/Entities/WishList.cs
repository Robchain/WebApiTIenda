namespace WebApiTIenda.Domian.Entities
{
    public class WishList
    {
        public int IdList { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; } = null!;

        public ICollection<WishListDetail> WishListDetails { get; set; } = new List<WishListDetail>();
    }
}
