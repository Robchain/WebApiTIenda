using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Interfaces.Admin
{
    public interface IProductSupplierAdminService
    {
        Task<ApiResponseDto<bool>> AssignSupplierToProductAsync(ProductSupplierCreateDto createDto);
        Task<ApiResponseDto<bool>> UpdateProductSupplierAsync(int productId, int supplierId, ProductSupplierUpdateDto updateDto);
        Task<ApiResponseDto<bool>> RemoveSupplierFromProductAsync(int productId, int supplierId);
        Task<ApiResponseDto<List<ProductSupplierDetailDto>>> GetProductSuppliersAsync(int productId);
    }
}
