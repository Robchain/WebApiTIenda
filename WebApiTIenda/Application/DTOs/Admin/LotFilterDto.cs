namespace WebApiTIenda.Application.DTOs.Admin
{
    public class LotFilterDto : PaginationDto
    {
        public string? CodeLote { get; set; }
        public DateTime? StartAdmissionDate { get; set; }
        public DateTime? EndAdmissionDate { get; set; }
        public DateTime? StartExpDate { get; set; }
        public DateTime? EndExpDate { get; set; }
        public bool? IsExpired { get; set; }
        public bool? ExpiringSoon { get; set; }
    }
}
