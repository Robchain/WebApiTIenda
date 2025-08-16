using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Domian.Exceptions;

namespace WebApiTIenda.Controllers.buyer
{
    [ApiController]
    [Route("api/buyer/[controller]")]
    [Authorize]
    public class ProductsControllerBuyer : Controller
    {
        private readonly IProductBuyerService _productService;
        private readonly ILogger<ProductsControllerBuyer> _logger;

        public ProductsControllerBuyer(IProductBuyerService productService, ILogger<ProductsControllerBuyer> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Application.DTOs.Buyer.ProductListDto>>>> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos para comprador");
                return StatusCode(500, new ApiResponseDto<List<Application.DTOs.Buyer.ProductListDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Application.DTOs.Buyer.ProductListDto>>> GetProduct(int id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _productService.GetProductWithPricesAsync(id, userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<Application.DTOs.Buyer.ProductListDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto {ProductId}", id);
                return StatusCode(500, new ApiResponseDto<Application.DTOs.Buyer.ProductListDto>
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
