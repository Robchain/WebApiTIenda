using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Interfaces.Admin
{
    public interface IProductAdminService
    {
        Task<ApiResponseDto<List<ProductListDto>>> GetAllProductsAsync();
        Task<ApiResponseDto<ProductDetailDto>> GetProductDetailAsync(int productId);
        Task<ApiResponseDto<ProductDetailDto>> CreateProductAsync(ProductCreateDto createDto);
        Task<ApiResponseDto<ProductDetailDto>> UpdateProductAsync(int productId, ProductUpdateDto updateDto);
        Task<ApiResponseDto<bool>> DeleteProductAsync(int productId);
    }
}
