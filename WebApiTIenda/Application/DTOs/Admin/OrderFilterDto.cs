namespace WebApiTIenda.Application.DTOs.Admin
{
    public class OrderFilterDto : PaginationDto
    {
        public int? IdUser { get; set; }
        public string? UserName { get; set; }
        public string? State { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }
    }
}
