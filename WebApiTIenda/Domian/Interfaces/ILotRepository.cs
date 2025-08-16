using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface ILotRepository : IGenericRepository<Lot>
    {
        Task<Lot?> GetLotWithProductsAsync(int lotId);
    }
}
