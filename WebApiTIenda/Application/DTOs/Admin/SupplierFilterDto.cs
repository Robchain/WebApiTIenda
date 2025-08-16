namespace WebApiTIenda.Application.DTOs.Admin
{
    public class SupplierFilterDto : PaginationDto
    {
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public bool? IsActive { get; set; }
    }
}
