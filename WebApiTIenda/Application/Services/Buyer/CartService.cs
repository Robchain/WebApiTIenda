using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Shared.Helpers;

namespace WebApiTIenda.Application.Services.Buyer
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(IUnitOfWork unitOfWork, ICartRepository cartRepository, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<CartItemDto>>> GetCartItemsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Obteniendo items del carrito para usuario {UserId}", userId);

                var cartItems = await _cartRepository.GetCartItemsAsync(userId);
                var cartItemDtos = cartItems.Select(ci => new CartItemDto
                {
                    IdProduct = ci.ProductId,
                    ProductName = ci.ProductName,
                    ProductImageUrl = "", // Se obtendría del producto
                    IdSupplier = ci.SupplierId,
                    SupplierName = ci.SupplierName,
                    UnitPrice = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    AvailableStock = ci.AvailableStock
                }).ToList();

                return new ApiResponseDto<List<CartItemDto>>
                {
                    Success = true,
                    Message = "Items del carrito obtenidos exitosamente",
                    Data = cartItemDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener items del carrito para usuario {UserId}", userId);
                return new ApiResponseDto<List<CartItemDto>>
                {
                    Success = false,
                    Message = "Error al obtener items del carrito",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> AddToCartAsync(int userId, AddToCartDto addToCartDto)
        {
            try
            {
                _logger.LogInformation("Agregando producto {ProductId} al carrito del usuario {UserId}",
                    addToCartDto.IdProduct, userId);

                // Verificar que el producto y proveedor existan
                var productSupplier = await _unitOfWork.ProductSuppliers
                    .GetProductSupplierAsync(addToCartDto.IdProduct, addToCartDto.IdSupplier);

                if (productSupplier == null)
                    throw new NotFoundException("Producto o proveedor no encontrado");

                // Verificar stock disponible
                if (productSupplier.Stock < addToCartDto.Quantity)
                    throw new InvalidOperationException($"Stock insuficiente. Disponible: {productSupplier.Stock}");

                // Verificar que el lote no esté vencido
                if (productSupplier.Lot.ExpDate < DateTime.Now)
                    throw new InvalidOperationException("El lote del producto está vencido");

                // Crear item del carrito
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = addToCartDto.IdProduct,
                    SupplierId = addToCartDto.IdSupplier,
                    Quantity = addToCartDto.Quantity,
                    UnitPrice = productSupplier.Price,
                    ProductName = productSupplier.Product.Name,
                    SupplierName = productSupplier.Supplier.Name,
                    AvailableStock = productSupplier.Stock
                };

                var result = await _cartRepository.AddItemAsync(cartItem);

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = result ? "Producto agregado al carrito exitosamente" : "Error al agregar producto al carrito",
                    Data = result
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al carrito");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al agregar producto al carrito",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> RemoveFromCartAsync(int userId, int productId, int supplierId)
        {
            try
            {
                _logger.LogInformation("Removiendo producto {ProductId} del carrito del usuario {UserId}",
                    productId, userId);

                var result = await _cartRepository.RemoveItemAsync(userId, productId, supplierId);

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = result ? "Producto removido del carrito exitosamente" : "Producto no encontrado en el carrito",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover producto del carrito");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al remover producto del carrito",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> ClearCartAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Limpiando carrito del usuario {UserId}", userId);

                var result = await _cartRepository.ClearCartAsync(userId);

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = "Carrito limpiado exitosamente",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar carrito del usuario {UserId}", userId);
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al limpiar carrito",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
