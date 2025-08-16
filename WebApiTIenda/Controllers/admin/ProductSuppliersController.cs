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
    public class ProductSuppliersController : ControllerBase
    {
        private readonly IProductSupplierAdminService _productSupplierService;
        private readonly ILogger<ProductSuppliersController> _logger;

        public ProductSuppliersController(IProductSupplierAdminService productSupplierService, ILogger<ProductSuppliersController> logger)
        {
            _productSupplierService = productSupplierService;
            _logger = logger;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ApiResponseDto<List<ProductSupplierDetailDto>>>> GetProductSuppliers(int productId)
        {
            try
            {
                var result = await _productSupplierService.GetProductSuppliersAsync(productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores del producto {ProductId}", productId);
                return StatusCode(500, new ApiResponseDto<List<ProductSupplierDetailDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<bool>>> AssignSupplierToProduct([FromBody] ProductSupplierCreateDto createDto)
        {
            try
            {
                var result = await _productSupplierService.AssignSupplierToProductAsync(createDto);
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
                _logger.LogError(ex, "Error al asignar proveedor a producto");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{productId}/{supplierId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UpdateProductSupplier(int productId, int supplierId, [FromBody] ProductSupplierUpdateDto updateDto)
        {
            try
            {
                var result = await _productSupplierService.UpdateProductSupplierAsync(productId, supplierId, updateDto);
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
                _logger.LogError(ex, "Error al actualizar relación producto-proveedor");
                return StatusCode(500, new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{productId}/{supplierId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveSupplierFromProduct(int productId, int supplierId)
        {
            try
            {
                var result = await _productSupplierService.RemoveSupplierFromProductAsync(productId, supplierId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover proveedor del producto");
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
