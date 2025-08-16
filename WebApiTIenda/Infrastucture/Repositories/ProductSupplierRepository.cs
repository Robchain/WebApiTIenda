using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;

namespace WebApiTIenda.Infrastucture.Repositories
{
    public class ProductSupplierRepository : IProductSupplierRepository
    {
        private readonly AppDbContext _context;

        public ProductSupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSupplier>> GetProductSuppliersAsync(int productId)
        {
            return await _context.Set<ProductSupplier>()
                .Include(ps => ps.Supplier)
                .Include(ps => ps.Lot)
                .Where(ps => ps.IdProduct == productId)
                .ToListAsync();
        }

        public async Task<List<ProductSupplier>> GetSupplierProductsAsync(int supplierId)
        {
            return await _context.Set<ProductSupplier>()
                .Include(ps => ps.Product)
                .Include(ps => ps.Lot)
                .Where(ps => ps.IdSupplier == supplierId)
                .ToListAsync();
        }

        public async Task<ProductSupplier?> GetProductSupplierAsync(int productId, int supplierId)
        {
            return await _context.Set<ProductSupplier>()
                .Include(ps => ps.Product)
                .Include(ps => ps.Supplier)
                .Include(ps => ps.Lot)
                .FirstOrDefaultAsync(ps => ps.IdProduct == productId && ps.IdSupplier == supplierId);
        }

        public async Task<ProductSupplier> AddAsync(ProductSupplier productSupplier)
        {
            await _context.Set<ProductSupplier>().AddAsync(productSupplier);
            return productSupplier;
        }

        public async Task<ProductSupplier> UpdateAsync(ProductSupplier productSupplier)
        {
            _context.Set<ProductSupplier>().Update(productSupplier);
            return productSupplier;
        }

        public async Task<bool> RemoveAsync(int productId, int supplierId)
        {
            var productSupplier = await _context.Set<ProductSupplier>()
                .FirstOrDefaultAsync(ps => ps.IdProduct == productId && ps.IdSupplier == supplierId);

            if (productSupplier == null)
                return false;

            _context.Set<ProductSupplier>().Remove(productSupplier);
            return true;
        }

        public async Task<bool> UpdateStockAsync(int productId, int supplierId, int newStock)
        {
            var productSupplier = await _context.Set<ProductSupplier>()
                .FirstOrDefaultAsync(ps => ps.IdProduct == productId && ps.IdSupplier == supplierId);

            if (productSupplier == null)
                return false;

            productSupplier.Stock = newStock;
            return true;
        }

        public async Task<bool> UpdatePriceAsync(int productId, int supplierId, decimal newPrice)
        {
            var productSupplier = await _context.Set<ProductSupplier>()
                .FirstOrDefaultAsync(ps => ps.IdProduct == productId && ps.IdSupplier == supplierId);

            if (productSupplier == null)
                return false;

            productSupplier.Price = newPrice;
            return true;
        }
    }
}
