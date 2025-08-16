using WebApiTIenda.Shared.Helpers;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartItemsAsync(int userId);
        Task<bool> AddItemAsync(CartItem item);
        Task<bool> UpdateItemAsync(CartItem item);
        Task<bool> RemoveItemAsync(int userId, int productId, int supplierId);
        Task<bool> ClearCartAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int productId, int supplierId);
    }
}
