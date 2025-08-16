using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderR>
    {
        Task<OrderR?> GetOrderWithDetailsAsync(int orderId);
        Task<List<OrderR>> GetUserOrdersAsync(int userId);
    }
}
