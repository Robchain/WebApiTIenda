namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class MyWishListDto
    {
        public int IdList { get; set; }
        public List<WishListItemDto> Items { get; set; } = new();
        public int TotalItems { get; set; }
    }
}
