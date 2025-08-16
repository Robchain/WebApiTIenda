using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Domian.Exceptions;

namespace WebApiTIenda.Controllers.buyer
{
    [ApiController]
    [Route("api/buyer/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<CartItemDto>>>> GetCartItems()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _cartService.GetCartItemsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener items del carrito");
                return StatusCode(500, new ApiResponseDto<List<CartItemDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _cartService.AddToCartAsync(userId, addToCartDto);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar al carrito");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{productId}/{supplierId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveFromCart(int productId, int supplierId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _cartService.RemoveFromCartAsync(userId, productId, supplierId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover del carrito");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("clear")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ClearCart()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _cartService.ClearCartAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar carrito");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
