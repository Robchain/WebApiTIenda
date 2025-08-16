namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductSupplierDetailDto
    {
        public int IdSupplier { get; set; }
        public string SupplierName { get; set; } = null!;
        public string SupplierContact { get; set; } = null!;
        public string SupplierPhone { get; set; } = null!;
        public int IdLot { get; set; }
        public string LotCode { get; set; } = null!;
        public DateTime LotExpDate { get; set; }
        public bool IsLotExpired { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
