using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Domian.Exceptions;

namespace WebApiTIenda.Controllers.admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductAdminService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductAdminService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<ProductListDto>>>> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos para admin");
                return StatusCode(500, new ApiResponseDto<List<ProductListDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProductDetailDto>>> GetProduct(int id)
        {
            try
            {
                var result = await _productService.GetProductDetailAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto {ProductId}", id);
                return StatusCode(500, new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<ProductDetailDto>>> CreateProduct([FromBody] ProductCreateDto createDto)
        {
            try
            {
                var result = await _productService.CreateProductAsync(createDto);
                return CreatedAtAction(nameof(GetProduct), new { id = result.Data?.IdProduct }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProductDetailDto>>> UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(id, updateDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {ProductId}", id);
                return StatusCode(500, new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
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
                _logger.LogError(ex, "Error al eliminar producto {ProductId}", id);
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
