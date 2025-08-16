using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<Supplier?> GetSupplierWithProductsAsync(int supplierId);
    }
}
