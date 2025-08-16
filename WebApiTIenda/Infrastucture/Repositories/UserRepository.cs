using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Enums;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserWithWishListsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.WishLists)
                    .ThenInclude(w => w.WishListDetails)
                        .ThenInclude(wd => wd.Product)
                .FirstOrDefaultAsync(u => u.IdUser == userId);
        }

        public async Task<User?> GetUserWithOrdersAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.OrderRs)
                    .ThenInclude(o => o.OrderRDetails)
                        .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(u => u.IdUser == userId);
        }

        public async Task<List<User>> GetUsersByRolAsync(UserRoles rol)
        {
            return await _dbSet
                .Where(u => u.Rol == rol)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user == null)
                return false;
            _dbSet.Update(user);
            return true;
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.WishLists)
                .Include(u => u.OrderRs)
                .FirstOrDefaultAsync(u => u.IdUser == id);
        }

        public override async Task<List<User>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}
