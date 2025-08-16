namespace WebApiTIenda.Application.DTOs.Admin
{
    public class LotListDto
    {
        public int IdLot { get; set; }
        public string CodeLote { get; set; } = null!;
        public DateTime DateOfAdmission { get; set; }
        public DateTime ExpDate { get; set; }
        public bool IsExpired { get; set; }
        public int DaysUntilExpiration { get; set; }
        public string ExpirationStatus { get; set; } = null!;
        public int TotalProducts { get; set; }
    }
}
