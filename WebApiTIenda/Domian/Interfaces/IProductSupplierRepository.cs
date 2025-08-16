using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface IProductSupplierRepository
    {
        Task<List<ProductSupplier>> GetProductSuppliersAsync(int productId);
        Task<List<ProductSupplier>> GetSupplierProductsAsync(int supplierId);
        Task<ProductSupplier?> GetProductSupplierAsync(int productId, int supplierId);
        Task<ProductSupplier> AddAsync(ProductSupplier productSupplier);
        Task<ProductSupplier> UpdateAsync(ProductSupplier productSupplier);
        Task<bool> RemoveAsync(int productId, int supplierId);
        Task<bool> UpdateStockAsync(int productId, int supplierId, int newStock);
        Task<bool> UpdatePriceAsync(int productId, int supplierId, decimal newPrice);
    }
}
