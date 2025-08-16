namespace WebApiTIenda.Domian.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
