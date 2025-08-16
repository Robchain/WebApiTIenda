using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Enums;

namespace WebApiTIenda.Domian.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Busca un usuario por su email
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Verifica si existe un usuario con el email especificado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True si existe, False si no existe</returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Obtiene un usuario con sus listas de deseos incluidas
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Usuario con sus wishlist</returns>
        Task<User?> GetUserWithWishListsAsync(int userId);

        /// <summary>
        /// Obtiene un usuario con sus órdenes incluidas
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Usuario con sus órdenes</returns>
        Task<User?> GetUserWithOrdersAsync(int userId);

        /// <summary>
        /// Obtiene usuarios por rol
        /// </summary>
        /// <param name="rol">Rol a filtrar</param>
        /// <returns>Lista de usuarios con el rol especificado</returns>
        Task<List<User>> GetUsersByRolAsync(UserRoles rol);

        /// <summary>
        /// Actualiza la fecha de último login del usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> UpdateLastLoginAsync(int userId);
    }
}
