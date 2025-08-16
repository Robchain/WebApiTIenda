namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class WishListItemDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        public string ProductImageUrl { get; set; } = null!;
        public DateTime DateAdded { get; set; }
        public decimal BestPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
