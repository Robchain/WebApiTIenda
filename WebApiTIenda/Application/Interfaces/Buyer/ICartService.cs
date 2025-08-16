using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;

namespace WebApiTIenda.Application.Interfaces.Buyer
{
    public interface ICartService
    {
        Task<ApiResponseDto<List<CartItemDto>>> GetCartItemsAsync(int userId);
        Task<ApiResponseDto<bool>> AddToCartAsync(int userId, AddToCartDto addToCartDto);
        Task<ApiResponseDto<bool>> RemoveFromCartAsync(int userId, int productId, int supplierId);
        Task<ApiResponseDto<bool>> ClearCartAsync(int userId);
    }
}
