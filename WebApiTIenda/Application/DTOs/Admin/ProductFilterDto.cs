namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ProductFilterDto : PaginationDto
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public bool? HasStock { get; set; }
        public bool? HasSuppliers { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
