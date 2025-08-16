using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;

namespace WebApiTIenda.Application.Interfaces.Buyer
{
    public interface IWishListService
    {
        Task<ApiResponseDto<List<WishListItemDto>>> GetWishListAsync(int userId);
        Task<ApiResponseDto<bool>> AddToWishListAsync(int userId, int productId);
        Task<ApiResponseDto<bool>> RemoveFromWishListAsync(int userId, int productId);
    }
}
