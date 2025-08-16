using AutoMapper;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;

namespace WebApiTIenda.Application.Services.Admin
{
    public class LotAdminService : ILotAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LotAdminService> _logger;

        public LotAdminService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LotAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponseDto<List<LotListDto>>> GetAllLotsAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los lotes");

                var lots = await _unitOfWork.Lots.GetAllAsync();
                var lotDtos = lots.Select(l => new LotListDto
                {
                    IdLot = l.IdLot,
                    CodeLote = l.CodeLote,
                    DateOfAdmission = l.DateOfAdmission,
                    ExpDate = l.ExpDate,
                    IsExpired = l.ExpDate < DateTime.Now,
                    DaysUntilExpiration = (l.ExpDate - DateTime.Now).Days,
                    ExpirationStatus = l.ExpDate < DateTime.Now ? "Vencido" :
                                     (l.ExpDate - DateTime.Now).Days <= 30 ? "Próximo a vencer" : "Vigente",
                    TotalProducts = l.ProductSuppliers?.Count ?? 0
                }).ToList();

                return new ApiResponseDto<List<LotListDto>>
                {
                    Success = true,
                    Message = "Lotes obtenidos exitosamente",
                    Data = lotDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lotes");
                return new ApiResponseDto<List<LotListDto>>
                {
                    Success = false,
                    Message = "Error al obtener lotes",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<LotDetailDto>> CreateLotAsync(LotCreateDto createDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo lote: {LotCode}", createDto.CodeLote);

                // Validar fechas
                if (createDto.ExpDate <= createDto.DateOfAdmission)
                    throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de admisión");

                var lot = new Lot
                {
                    CodeLote = createDto.CodeLote,
                    DateOfAdmission = createDto.DateOfAdmission,
                    ExpDate = createDto.ExpDate
                };

                await _unitOfWork.Lots.AddAsync(lot);
                await _unitOfWork.SaveChangesAsync();

                var lotDetailDto = new LotDetailDto
                {
                    IdLot = lot.IdLot,
                    CodeLote = lot.CodeLote,
                    DateOfAdmission = lot.DateOfAdmission,
                    ExpDate = lot.ExpDate,
                    IsExpired = lot.ExpDate < DateTime.Now,
                    DaysUntilExpiration = (lot.ExpDate - DateTime.Now).Days,
                    ExpirationStatus = lot.ExpDate < DateTime.Now ? "Vencido" : "Vigente",
                    Products = new List<LotProductDto>()
                };

                return new ApiResponseDto<LotDetailDto>
                {
                    Success = true,
                    Message = "Lote creado exitosamente",
                    Data = lotDetailDto
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear lote");
                return new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = "Error al crear lote",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<LotDetailDto>> UpdateLotAsync(int lotId, LotUpdateDto updateDto)
        {
            try
            {
                _logger.LogInformation("Actualizando lote {LotId}", lotId);

                var lot = await _unitOfWork.Lots.GetByIdAsync(lotId);
                if (lot == null)
                    throw new NotFoundException($"Lote con ID {lotId} no encontrado");

                // Validar fechas
                if (updateDto.ExpDate <= updateDto.DateOfAdmission)
                    throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de admisión");

                lot.CodeLote = updateDto.CodeLote;
                lot.DateOfAdmission = updateDto.DateOfAdmission;
                lot.ExpDate = updateDto.ExpDate;

                await _unitOfWork.Lots.UpdateAsync(lot);
                await _unitOfWork.SaveChangesAsync();

                var lotDetailDto = _mapper.Map<LotDetailDto>(lot);

                return new ApiResponseDto<LotDetailDto>
                {
                    Success = true,
                    Message = "Lote actualizado exitosamente",
                    Data = lotDetailDto
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                return new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar lote {LotId}", lotId);
                return new ApiResponseDto<LotDetailDto>
                {
                    Success = false,
                    Message = "Error al actualizar lote",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ApiResponseDto<bool>> DeleteLotAsync(int lotId)
        {
            try
            {
                _logger.LogInformation("Eliminando lote {LotId}", lotId);

                var lot = await _unitOfWork.Lots.GetLotWithProductsAsync(lotId);
                if (lot == null)
                    throw new NotFoundException($"Lote con ID {lotId} no encontrado");

                if (lot.ProductSuppliers.Any())
                    throw new InvalidOperationException("No se puede eliminar un lote que tiene productos asignados");

                var result = await _unitOfWork.Lots.DeleteAsync(lotId);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = "Lote eliminado exitosamente",
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
                _logger.LogError(ex, "Error al eliminar lote {LotId}", lotId);
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al eliminar lote",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
