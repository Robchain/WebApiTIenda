using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Auth;

namespace WebApiTIenda.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">Datos del usuario a registrar</param>
        /// <returns>Respuesta con datos del usuario registrado</returns>
        Task<ApiResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        /// <param name="loginDto">Credenciales del usuario</param>
        /// <returns>Respuesta con token y datos del usuario</returns>
        Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Obtiene el perfil de un usuario autenticado
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Datos del perfil del usuario</returns>
        Task<ApiResponseDto<UserProfileDto>> GetUserProfileAsync(int userId);

        /// <summary>
        /// Verifica si un email ya está registrado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True si el email existe, False si no</returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Valida las credenciales de un usuario
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña sin encriptar</param>
        /// <returns>Usuario si las credenciales son válidas, null si no</returns>
        Task<Domian.Entities.User?> ValidateUserCredentialsAsync(string email, string password);
    }
}
