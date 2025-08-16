namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class ProductDetailDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public List<ProductPriceOptionDto> PriceOptions { get; set; } = new(); // Opciones de precios por proveedor
        public decimal BestPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsInWishList { get; set; } // Si está en la wishlist del usuario
    }
}
