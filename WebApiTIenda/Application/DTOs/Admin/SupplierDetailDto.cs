namespace WebApiTIenda.Application.DTOs.Admin
{
    public class SupplierDetailDto
    {
        public int IdSupplier { get; set; }
        public string Name { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public List<SupplierProductDto> Products { get; set; } = new();
        public int TotalProducts { get; set; }
    }
}
