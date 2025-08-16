namespace WebApiTIenda.Application.DTOs.Admin
{
    public class LotDetailDto
    {
        public int IdLot { get; set; }
        public string CodeLote { get; set; } = null!;
        public DateTime DateOfAdmission { get; set; }
        public DateTime ExpDate { get; set; }
        public bool IsExpired { get; set; }
        public int DaysUntilExpiration { get; set; }
        public string ExpirationStatus { get; set; } = null!;
        public List<LotProductDto> Products { get; set; } = new();
    }
}
