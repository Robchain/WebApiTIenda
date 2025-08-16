namespace WebApiTIenda.Domian.Interfaces
{
    /// <summary>
    /// Unit of Work pattern para gestionar transacciones y repositorios
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        // ============== REPOSITORIOS ESPECÍFICOS ==============

        /// <summary>
        /// Repositorio de productos
        /// </summary>
        IProductRepository Products { get; }

        /// <summary>
        /// Repositorio de órdenes
        /// </summary>
        IOrderRepository Orders { get; }

        /// <summary>
        /// Repositorio de proveedores
        /// </summary>
        ISupplierRepository Suppliers { get; }

        /// <summary>
        /// Repositorio de lotes
        /// </summary>
        ILotRepository Lots { get; }

        /// <summary>
        /// Repositorio de listas de deseos
        /// </summary>
        IWishListRepository WishLists { get; }

        /// <summary>
        /// Repositorio de relaciones producto-proveedor
        /// </summary>
        IProductSupplierRepository ProductSuppliers { get; }

        /// <summary>
        /// Repositorio de usuarios
        /// </summary>
        IUserRepository Users { get; }

        // ============== REPOSITORIO GENÉRICO ==============

        /// <summary>
        /// Obtiene un repositorio genérico para cualquier entidad
        /// </summary>
        /// <typeparam name="T">Tipo de entidad</typeparam>
        /// <returns>Repositorio genérico para la entidad</returns>
        IGenericRepository<T> Repository<T>() where T : class;

        // ============== MÉTODOS DE TRANSACCIONES ==============

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        /// <returns>Número de entidades afectadas</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción explícita
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Ejecuta una operación dentro de una transacción
        /// </summary>
        /// <param name="operation">Operación a ejecutar</param>
        /// <returns>True si la operación fue exitosa</returns>
        Task<bool> ExecuteInTransactionAsync(Func<Task> operation);

        /// <summary>
        /// Ejecuta una operación dentro de una transacción y retorna un resultado
        /// </summary>
        /// <typeparam name="T">Tipo del resultado</typeparam>
        /// <param name="operation">Operación a ejecutar</param>
        /// <returns>Resultado de la operación</returns>
        Task<T?> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);

        // ============== MÉTODOS AUXILIARES ==============

        /// <summary>
        /// Verifica si hay cambios pendientes
        /// </summary>
        /// <returns>True si hay cambios pendientes</returns>
        bool HasPendingChanges();

        /// <summary>
        /// Descarta todos los cambios pendientes
        /// </summary>
        void DiscardChanges();

        /// <summary>
        /// Obtiene el número de entidades en seguimiento
        /// </summary>
        /// <returns>Número de entidades en seguimiento</returns>
        int GetTrackedEntitiesCount();

        /// <summary>
        /// Limpia el ChangeTracker para mejorar el rendimiento
        /// </summary>
        void ClearChangeTracker();
    }
}
