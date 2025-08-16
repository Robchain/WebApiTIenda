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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierAdminService _supplierService;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(ISupplierAdminService supplierService, ILogger<SuppliersController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<SupplierListDto>>>> GetAllSuppliers()
        {
            try
            {
                var result = await _supplierService.GetAllSuppliersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores");
                return StatusCode(500, new ApiResponseDto<List<SupplierListDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<SupplierDetailDto>>> CreateSupplier([FromBody] SupplierCreateDto createDto)
        {
            try
            {
                var result = await _supplierService.CreateSupplierAsync(createDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear proveedor");
                return StatusCode(500, new ApiResponseDto<SupplierDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<SupplierDetailDto>>> UpdateSupplier(int id, [FromBody] SupplierUpdateDto updateDto)
        {
            try
            {
                var result = await _supplierService.UpdateSupplierAsync(id, updateDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<SupplierDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar proveedor {SupplierId}", id);
                return StatusCode(500, new ApiResponseDto<SupplierDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteSupplier(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(id);
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
                _logger.LogError(ex, "Error al eliminar proveedor {SupplierId}", id);
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
