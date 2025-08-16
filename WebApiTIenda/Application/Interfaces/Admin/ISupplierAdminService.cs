using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Interfaces.Admin
{
    public interface ISupplierAdminService
    {
        Task<ApiResponseDto<List<SupplierListDto>>> GetAllSuppliersAsync();
        Task<ApiResponseDto<SupplierDetailDto>> CreateSupplierAsync(SupplierCreateDto createDto);
        Task<ApiResponseDto<SupplierDetailDto>> UpdateSupplierAsync(int supplierId, SupplierUpdateDto updateDto);
        Task<ApiResponseDto<bool>> DeleteSupplierAsync(int supplierId);
    }
}
