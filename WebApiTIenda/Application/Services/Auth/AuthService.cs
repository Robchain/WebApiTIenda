using AutoMapper;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Auth;
using WebApiTIenda.Application.Interfaces.Auth;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastucture.Security;

namespace WebApiTIenda.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly PasswordHasher _passwordHasher;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AuthService> logger,
            JwtTokenGenerator jwtTokenGenerator,
            PasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Iniciando registro de usuario: {Email}", registerDto.Email);

                // Verificar si el email ya existe
                if (await EmailExistsAsync(registerDto.Email))
                {
                    _logger.LogWarning("Intento de registro con email duplicado: {Email}", registerDto.Email);
                    return new ApiResponseDto<RegisterResponseDto>
                    {
                        Success = false,
                        Message = "El email ya está registrado",
                        Errors = new List<string> { "Ya existe un usuario con este email" }
                    };
                }

                // Crear nuevo usuario
                var user = new User
                {
                    Name = registerDto.Name.Trim(),
                    Email = registerDto.Email.Trim().ToLower(),
                    Password = _passwordHasher.PasswordEncrip(registerDto.Password),
                    Rol = registerDto.Rol,
                    DateCreated = DateTime.Now
                };

                // Guardar usuario usando repositorio específico
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado exitosamente: {UserId} - {Email}", user.IdUser, user.Email);

                // Si es comprador, crear su wishlist automáticamente
                if (user.Rol == Domian.Enums.UserRoles.Comprador)
                {
                    try
                    {
                        var wishList = new WishList
                        {
                            IdUser = user.IdUser
                        };
                        await _unitOfWork.WishLists.AddAsync(wishList);
                        await _unitOfWork.SaveChangesAsync();

                        _logger.LogInformation("WishList creada automáticamente para usuario: {UserId}", user.IdUser);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error al crear wishlist automática para usuario: {UserId}", user.IdUser);
                        // No fallar el registro por esto
                    }
                }

                // Mapear respuesta
                var response = new RegisterResponseDto
                {
                    IdUser = user.IdUser,
                    Name = user.Name,
                    Email = user.Email,
                    Rol = user.Rol.ToString(),
                    DateCreated = user.DateCreated
                };

                return new ApiResponseDto<RegisterResponseDto>
                {
                    Success = true,
                    Message = "Usuario registrado exitosamente",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario: {Email}", registerDto.Email);
                return new ApiResponseDto<RegisterResponseDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error al registrar el usuario" }
                };
            }
        }

        public async Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Intento de login para: {Email}", loginDto.Email);

                // Validar credenciales usando repositorio específico
                var user = await ValidateUserCredentialsAsync(loginDto.Email, loginDto.Password);

                if (user == null)
                {
                    _logger.LogWarning("Credenciales inválidas para: {Email}", loginDto.Email);
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Credenciales inválidas",
                        Errors = new List<string> { "Email o contraseña incorrectos" }
                    };
                }

                // Generar token JWT
                var token = _jwtTokenGenerator.GetJWT(user);
                var loginTime = DateTime.Now;
                var tokenExpiration = DateTime.UtcNow.AddHours(2);

                _logger.LogInformation("Login exitoso para usuario: {UserId} - {Email}", user.IdUser, user.Email);

                var response = new LoginResponseDto
                {
                    IdUser = user.IdUser,
                    Name = user.Name,
                    Email = user.Email,
                    Rol = user.Rol.ToString(),
                    Token = token,
                    LoginTime = loginTime,
                    TokenExpiration = tokenExpiration
                };

                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = true,
                    Message = "Login exitoso",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login para: {Email}", loginDto.Email);
                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error al procesar el login" }
                };
            }
        }

        public async Task<ApiResponseDto<UserProfileDto>> GetUserProfileAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Obteniendo perfil de usuario: {UserId}", userId);

                var user = await _unitOfWork.Users.GetByIdAsync(userId);

                if (user == null)
                {
                    throw new NotFoundException($"Usuario con ID {userId} no encontrado");
                }

                var profile = new UserProfileDto
                {
                    IdUser = user.IdUser,
                    Name = user.Name,
                    Email = user.Email,
                    Rol = user.Rol.ToString(),
                    DateCreated = user.DateCreated
                };

                return new ApiResponseDto<UserProfileDto>
                {
                    Success = true,
                    Message = "Perfil obtenido exitosamente",
                    Data = profile
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil de usuario: {UserId}", userId);
                return new ApiResponseDto<UserProfileDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { "Ocurrió un error al obtener el perfil" }
                };
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                return await _unitOfWork.Users.ExistsByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de email: {Email}", email);
                return false;
            }
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation("Validando credenciales para: {Email}", email);

                // Usar repositorio específico directamente
                var user = await _unitOfWork.Users.GetByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogInformation("Usuario no encontrado: {Email}", email);
                    return null;
                }

                // Verificar contraseña
                var hashedPassword = _passwordHasher.PasswordEncrip(password);
                if (user.Password != hashedPassword)
                {
                    _logger.LogInformation("Contraseña incorrecta para: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Credenciales válidas para usuario: {UserId} - {Email}", user.IdUser, user.Email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar credenciales para: {Email}", email);
                return null;
            }
        }
    }
}
