using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Data;
using WebApiTIenda.Infrastucture.Repositories;

namespace WebApiTIenda.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Lazy loading de repositorios específicos
        private IProductRepository? _products;
        private IOrderRepository? _orders;
        private ISupplierRepository? _suppliers;
        private ILotRepository? _lots;
        private IWishListRepository? _wishLists;
        private IProductSupplierRepository? _productSuppliers;
        private IUserRepository? _users;

        // Dictionary para repositorios genéricos
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // ============== PROPIEDADES DE REPOSITORIOS ESPECÍFICOS ==============

        public IProductRepository Products =>
            _products ??= new ProductRepository(_context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(_context);

        public ISupplierRepository Suppliers =>
            _suppliers ??= new SupplierRepository(_context);

        public ILotRepository Lots =>
            _lots ??= new LotRepository(_context);

        public IWishListRepository WishLists =>
            _wishLists ??= new WishListRepository(_context);

        public IProductSupplierRepository ProductSuppliers =>
            _productSuppliers ??= new ProductSupplierRepository(_context);

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);

        // ============== REPOSITORIO GENÉRICO ==============

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<T>)_repositories[type];
            }

            // Crear repositorio específico si existe, sino genérico
            IGenericRepository<T> repository = type.Name switch
            {
                nameof(Product) => (IGenericRepository<T>)Products,
                nameof(User) => (IGenericRepository<T>)Users,
                nameof(OrderR) => (IGenericRepository<T>)Orders,
                nameof(Supplier) => (IGenericRepository<T>)Suppliers,
                nameof(Lot) => (IGenericRepository<T>)Lots,
                nameof(WishList) => (IGenericRepository<T>)WishLists,
                _ => new GenericRepository<T>(_context)
            };

            _repositories.Add(type, repository);
            return repository;
        }

        // ============== MÉTODOS DE TRANSACCIONES ==============

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        /// <returns>Número de entidades afectadas</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Manejo de conflictos de concurrencia
                throw new InvalidOperationException("Los datos han sido modificados por otro usuario. Por favor, recarga la página e intenta nuevamente.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores de base de datos
                throw new InvalidOperationException("Error al guardar los cambios en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Inicia una transacción explícita
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay transacción activa para confirmar.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay transacción activa para revertir.");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Ejecuta una operación dentro de una transacción
        /// </summary>
        /// <param name="operation">Operación a ejecutar</param>
        /// <returns>True si la operación fue exitosa</returns>
        public async Task<bool> ExecuteInTransactionAsync(Func<Task> operation)
        {
            await BeginTransactionAsync();

            try
            {
                await operation();
                await SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch
            {
                await RollbackTransactionAsync();
                return false;
            }
        }

        /// <summary>
        /// Ejecuta una operación dentro de una transacción y retorna un resultado
        /// </summary>
        /// <typeparam name="T">Tipo del resultado</typeparam>
        /// <param name="operation">Operación a ejecutar</param>
        /// <returns>Resultado de la operación</returns>
        public async Task<T?> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            await BeginTransactionAsync();

            try
            {
                var result = await operation();
                await SaveChangesAsync();
                await CommitTransactionAsync();
                return result;
            }
            catch
            {
                await RollbackTransactionAsync();
                return default(T);
            }
        }

        // ============== MÉTODOS DE LIMPIEZA ==============

        /// <summary>
        /// Libera los recursos utilizados
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos de forma asíncrona
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }

        // ============== MÉTODOS ADICIONALES ÚTILES ==============

        /// <summary>
        /// Verifica si hay cambios pendientes
        /// </summary>
        /// <returns>True si hay cambios pendientes</returns>
        public bool HasPendingChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        /// <summary>
        /// Descarta todos los cambios pendientes
        /// </summary>
        public void DiscardChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        /// <summary>
        /// Obtiene el número de entidades en seguimiento
        /// </summary>
        /// <returns>Número de entidades en seguimiento</returns>
        public int GetTrackedEntitiesCount()
        {
            return _context.ChangeTracker.Entries().Count();
        }

        /// <summary>
        /// Limpia el ChangeTracker para mejorar el rendimiento
        /// </summary>
        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }
    }
}