using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface IWishListRepository : IGenericRepository<WishList>
    {
        Task<WishList?> GetUserWishListAsync(int userId);
        Task<bool> AddItemToWishListAsync(int listId, int productId);
        Task<bool> RemoveItemFromWishListAsync(int listId, int productId);
    }
}
