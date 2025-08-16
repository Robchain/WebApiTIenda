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
    public class LotsController : ControllerBase
    {
        private readonly ILotAdminService _lotService;
        private readonly ILogger<LotsController> _logger;

        public LotsController(ILotAdminService lotService, ILogger<LotsController> logger)
        {
            _lotService = lotService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<LotListDto>>>> GetAllLots()
        {
            try
            {
                var result = await _lotService.GetAllLotsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lotes");
                return StatusCode(500, new ApiResponseDto<List<LotListDto>>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<LotDetailDto>>> CreateLot([FromBody] LotCreateDto createDto)
        {
            try
            {
                var result = await _lotService.CreateLotAsync(createDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear lote");
                return StatusCode(500, new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<LotDetailDto>>> UpdateLot(int id, [FromBody] LotUpdateDto updateDto)
        {
            try
            {
                var result = await _lotService.UpdateLotAsync(id, updateDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar lote {LotId}", id);
                return StatusCode(500, new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = "Error interno del servidor",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteLot(int id)
        {
            try
            {
                var result = await _lotService.DeleteLotAsync(id);
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
                _logger.LogError(ex, "Error al eliminar lote {LotId}", id);
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
