using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class OrderRepository : GenericRepository<OrderR>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<OrderR?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderRDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);
        }

        public async Task<List<OrderR>> GetUserOrdersAsync(int userId)
        {
            return await _dbSet
                .Include(o => o.OrderRDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public override async Task<OrderR?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(o => o.User)
                .Include(o => o.OrderRDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }
    }
}
