using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Supplier?> GetSupplierWithProductsAsync(int supplierId)
        {
            return await _dbSet
                .Include(s => s.ProductSuppliers)
                    .ThenInclude(ps => ps.Product)
                .Include(s => s.ProductSuppliers)
                    .ThenInclude(ps => ps.Lot)
                .FirstOrDefaultAsync(s => s.IdSuPlier == supplierId);
        }

        public override async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.ProductSuppliers)
                    .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(s => s.IdSuPlier == id);
        }
    }
}
