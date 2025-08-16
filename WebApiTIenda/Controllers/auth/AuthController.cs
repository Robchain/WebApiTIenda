using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Auth;
using WebApiTIenda.Application.Interfaces.Auth;
using WebApiTIenda.Domian.Exceptions;

namespace WebApiTIenda.Controllers.auth
{
    /// <summary>
    /// Controller para manejo de autenticación y registro de usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">Datos del usuario a registrar</param>
        /// <returns>Información del usuario registrado</returns>
        /// <response code="200">Usuario registrado exitosamente</response>
        /// <response code="400">Datos inválidos o email duplicado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDto<RegisterResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<RegisterResponseDto>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<RegisterResponseDto>), 500)]
        public async Task<ActionResult<ApiResponseDto<RegisterResponseDto>>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponseDto<RegisterResponseDto>
                    {
                        Success = false,
                        Message = "Datos inválidos",
                        Errors = errors
                    });
                }

                _logger.LogInformation("Solicitud de registro recibida para: {Email}", registerDto.Email);

                var result = await _authService.RegisterAsync(registerDto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en registro de usuario");
                return StatusCode(500, new ApiResponseDto<RegisterResponseDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error inesperado" }
                });
            }
        }

        /// <summary>
        /// Autentica un usuario y genera un token de acceso
        /// </summary>
        /// <param name="loginDto">Credenciales del usuario</param>
        /// <returns>Token de acceso y datos del usuario</returns>
        /// <response code="200">Login exitoso</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="401">Credenciales incorrectas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), 400)]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), 401)]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), 500)]
        public async Task<ActionResult<ApiResponseDto<LoginResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Datos inválidos",
                        Errors = errors
                    });
                }

                _logger.LogInformation("Solicitud de login recibida para: {Email}", loginDto.Email);

                var result = await _authService.LoginAsync(loginDto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en login de usuario");
                return StatusCode(500, new ApiResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error inesperado" }
                });
            }
        }

        

        /// <summary>
        /// Endpoint para cerrar sesión (opcional, principalmente para logging)
        /// </summary>
        /// <returns>Confirmación de logout</returns>
        /// <response code="200">Logout exitoso</response>
        /// <response code="401">No autorizado</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 401)]
        public async Task<ActionResult<ApiResponseDto<string>>> Logout()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var userEmail = GetUserEmailFromToken();

                _logger.LogInformation("Logout para usuario: {UserId} - {Email}", userId, userEmail);

                return Ok(new ApiResponseDto<string>
                {
                    Success = true,
                    Message = "Logout exitoso",
                    Data = "Sesión cerrada correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en logout");
                return StatusCode(500, new ApiResponseDto<string>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error al cerrar sesión" }
                });
            }
        }

        // ============== MÉTODOS PRIVADOS AUXILIARES ==============

        /// <summary>
        /// Extrae el ID del usuario del token JWT
        /// </summary>
        /// <returns>ID del usuario o 0 si no se encuentra</returns>
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        /// <summary>
        /// Extrae el email del usuario del token JWT
        /// </summary>
        /// <returns>Email del usuario o string vacío si no se encuentra</returns>
        private string GetUserEmailFromToken()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value ?? string.Empty;
        }

        /// <summary>
        /// Extrae el rol del usuario del token JWT
        /// </summary>
        /// <returns>Rol del usuario o string vacío si no se encuentra</returns>
        private string GetUserRolFromToken()
        {
            var rolClaim = User.FindFirst(ClaimTypes.Role) ?? User.FindFirst("rol");
            return rolClaim?.Value ?? string.Empty;
        }

        /// <summary>
        /// Extrae el nombre del usuario del token JWT
        /// </summary>
        /// <returns>Nombre del usuario o string vacío si no se encuentra</returns>
        private string GetUserNameFromToken()
        {
            var nameClaim = User.FindFirst(ClaimTypes.Name);
            return nameClaim?.Value ?? string.Empty;
        }
    }
}