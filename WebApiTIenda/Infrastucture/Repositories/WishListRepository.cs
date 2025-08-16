using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class WishListRepository : GenericRepository<WishList>, IWishListRepository
    {
        public WishListRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WishList?> GetUserWishListAsync(int userId)
        {
            return await _dbSet
                .Include(w => w.WishListDetails)
                    .ThenInclude(wd => wd.Product)
                        .ThenInclude(p => p.ProductSuppliers)
                            .ThenInclude(ps => ps.Supplier)
                .FirstOrDefaultAsync(w => w.IdUser == userId);
        }

        public async Task<bool> AddItemToWishListAsync(int listId, int productId)
        {
            // Verificar si ya existe
            var existingItem = await _context.Set<WishListDetail>()
                .FirstOrDefaultAsync(wd => wd.IdList == listId && wd.IdProduct == productId);

            if (existingItem != null)
                return false; // Ya existe

            var wishListDetail = new WishListDetail
            {
                IdList = listId,
                IdProduct = productId,
                DateAdded = DateTime.Now
            };

            await _context.Set<WishListDetail>().AddAsync(wishListDetail);
            return true;
        }

        public async Task<bool> RemoveItemFromWishListAsync(int listId, int productId)
        {
            var item = await _context.Set<WishListDetail>()
                .FirstOrDefaultAsync(wd => wd.IdList == listId && wd.IdProduct == productId);

            if (item == null)
                return false;

            _context.Set<WishListDetail>().Remove(item);
            return true;
        }

        public override async Task<WishList?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(w => w.WishListDetails)
                    .ThenInclude(wd => wd.Product)
                .FirstOrDefaultAsync(w => w.IdList == id);
        }
    }
}
