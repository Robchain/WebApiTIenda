using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class LotRepository : GenericRepository<Lot>, ILotRepository
    {
        public LotRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Lot?> GetLotWithProductsAsync(int lotId)
        {
            return await _dbSet
                .Include(l => l.ProductSuppliers)
                    .ThenInclude(ps => ps.Product)
                .Include(l => l.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .FirstOrDefaultAsync(l => l.IdLot == lotId);
        }

        public override async Task<Lot?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(l => l.ProductSuppliers)
                    .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(l => l.IdLot == id);
        }
    }
}
