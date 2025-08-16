using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Interfaces.Buyer
{
    public interface IProductBuyerService
    {
        Task<ApiResponseDto<List<DTOs.Buyer.ProductListDto>>> GetAllProductsAsync();
        Task<ApiResponseDto<DTOs.Buyer.ProductListDto>> GetProductWithPricesAsync(int productId, int? userId = null);
    }
}
