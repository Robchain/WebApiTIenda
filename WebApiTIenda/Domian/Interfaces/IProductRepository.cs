using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetProductWithSuppliersAsync(int productId);
        Task<List<Product>> GetAllWithSuppliersAsync();
    }
}
