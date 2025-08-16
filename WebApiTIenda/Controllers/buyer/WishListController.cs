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
    public class WishListController : Controller
    {
        private readonly IWishListService _wishListService;
        private readonly ILogger<WishListController> _logger;

        public WishListController(IWishListService wishListService, ILogger<WishListController> logger)
        {
            _wishListService = wishListService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<WishListItemDto>>>> GetWishList()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _wishListService.GetWishListAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener wishlist");
                return StatusCode(500, new ApiResponseDto<List<WishListItemDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddToWishList(int productId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _wishListService.AddToWishListAsync(userId, productId);

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
                _logger.LogError(ex, "Error al agregar a wishlist");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveFromWishList(int productId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _wishListService.RemoveFromWishListAsync(userId, productId);
                return Ok(result);
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
                _logger.LogError(ex, "Error al remover de wishlist");
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
