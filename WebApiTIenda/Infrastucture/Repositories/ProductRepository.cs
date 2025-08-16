using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetProductWithSuppliersAsync(int productId)
        {
            return await _dbSet
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Lot)
                .FirstOrDefaultAsync(p => p.IdProduct == productId);
        }

        public async Task<List<Product>> GetAllWithSuppliersAsync()
        {
            return await _dbSet
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Lot)
                .ToListAsync();
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                .Include(p => p.ProductSuppliers)
                    .ThenInclude(ps => ps.Lot)
                .FirstOrDefaultAsync(p => p.IdProduct == id);
        }


    }
}
