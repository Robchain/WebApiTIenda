namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class ProductListDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal BestPrice { get; set; } // Mejor precio disponible
        public bool IsAvailable { get; set; } // Si hay stock disponible
        public int TotalStock { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
