namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class ProductPriceOptionDto
    {
        public int IdSupplier { get; set; }
        public string SupplierName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsAvailable => Stock > 0;
        public string LotCode { get; set; } = null!;
        public DateTime LotExpDate { get; set; }
        public bool IsLotValid => DateTime.Now < LotExpDate;
    }
}
