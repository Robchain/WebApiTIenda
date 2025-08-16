using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;

namespace WebApiTIenda.Application.Services.Buyer
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WishListService> _logger;

        public WishListService(IUnitOfWork unitOfWork, ILogger<WishListService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<WishListItemDto>>> GetWishListAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Obteniendo wishlist del usuario {UserId}", userId);

                var wishList = await _unitOfWork.WishLists.GetUserWishListAsync(userId);

                if (wishList == null)
                {
                    return new ApiResponseDto<List<WishListItemDto>>
                    {
                        Success = true,
                        Message = "Wishlist vacía",
                        Data = new List<WishListItemDto>()
                    };
                }

                var wishListItems = wishList.WishListDetails.Select(wd => new WishListItemDto
                {
                    IdProduct = wd.IdProduct,
                    ProductName = wd.Product.Name,
                    ProductCategory = wd.Product.Category,
                    ProductImageUrl = wd.Product.ImageUrl,
                    DateAdded = wd.DateAdded,
                    BestPrice = wd.Product.ProductSuppliers.Any() ?
                        wd.Product.ProductSuppliers.Min(ps => ps.Price) : 0,
                    IsAvailable = wd.Product.ProductSuppliers.Any(ps => ps.Stock > 0)
                }).ToList();

                return new ApiResponseDto<List<WishListItemDto>>
                {
                    Success = true,
                    Message = "Wishlist obtenida exitosamente",
                    Data = wishListItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener wishlist del usuario {UserId}", userId);
                return new ApiResponseDto<List<WishListItemDto>>
                {
                    Success = false,
                    Message = "Error al obtener wishlist",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> AddToWishListAsync(int userId, int productId)
        {
            try
            {
                _logger.LogInformation("Agregando producto {ProductId} a wishlist del usuario {UserId}",
                    productId, userId);

                // Verificar que el producto existe
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {productId} no encontrado");

                // Obtener o crear wishlist del usuario
                var wishList = await _unitOfWork.WishLists.GetUserWishListAsync(userId);
                if (wishList == null)
                {
                    wishList = new WishList { IdUser = userId };
                    await _unitOfWork.WishLists.AddAsync(wishList);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Agregar item a la wishlist
                var result = await _unitOfWork.WishLists.AddItemToWishListAsync(wishList.IdList, productId);

                if (result)
                    await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = result ? "Producto agregado a wishlist exitosamente" : "El producto ya está en la wishlist",
                    Data = result
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto a wishlist");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al agregar producto a wishlist",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> RemoveFromWishListAsync(int userId, int productId)
        {
            try
            {
                _logger.LogInformation("Removiendo producto {ProductId} de wishlist del usuario {UserId}",
                    productId, userId);

                var wishList = await _unitOfWork.WishLists.GetUserWishListAsync(userId);
                if (wishList == null)
                    throw new NotFoundException("Wishlist no encontrada");

                var result = await _unitOfWork.WishLists.RemoveItemFromWishListAsync(wishList.IdList, productId);

                if (result)
                    await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = result ? "Producto removido de wishlist exitosamente" : "Producto no encontrado en wishlist",
                    Data = result
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover producto de wishlist");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al remover producto de wishlist",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
