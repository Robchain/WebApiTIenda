namespace WebApiTIenda.Application.DTOs.Admin
{
    public class SupplierListDto
    {
        public int IdSupplier { get; set; }
        public string Name { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int TotalProducts { get; set; }
        public bool IsActive { get; set; }
    }
}
