using AutoMapper;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;

namespace WebApiTIenda.Application.Services.Admin
{
    public class SupplierAdminService : ISupplierAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierAdminService> _logger;

        public SupplierAdminService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SupplierAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<SupplierListDto>>> GetAllSuppliersAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los proveedores");

                var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
                var supplierDtos = suppliers.Select(s => new SupplierListDto
                {
                    IdSupplier = s.IdSuPlier,
                    Name = s.Name,
                    Contact = s.Contact,
                    Phone = s.Phone,
                    TotalProducts = s.ProductSuppliers?.Count ?? 0,
                    IsActive = s.ProductSuppliers?.Any(ps => ps.Stock > 0) ?? false
                }).ToList();

                return new ApiResponseDto<List<SupplierListDto>>
                {
                    Success = true,
                    Message = "Proveedores obtenidos exitosamente",
                    Data = supplierDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores");
                return new ApiResponseDto<List<SupplierListDto>>
                {
                    Success = false,
                    Message = "Error al obtener proveedores",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<SupplierDetailDto>> CreateSupplierAsync(SupplierCreateDto createDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo proveedor: {SupplierName}", createDto.Name);

                var supplier = new Supplier
                {
                    Name = createDto.Name,
                    Contact = createDto.Contact,
                    Phone = createDto.Phone,
                    Address = createDto.Address
                };

                await _unitOfWork.Suppliers.AddAsync(supplier);
                await _unitOfWork.SaveChangesAsync();

                var supplierDetailDto = new SupplierDetailDto
                {
                    IdSupplier = supplier.IdSuPlier,
                    Name = supplier.Name,
                    Contact = supplier.Contact,
                    Phone = supplier.Phone,
                    Address = supplier.Address,
                    Products = new List<SupplierProductDto>(),
                    TotalProducts = 0
                };

                return new ApiResponseDto<SupplierDetailDto>
                {
                    Success = true,
                    Message = "Proveedor creado exitosamente",
                    Data = supplierDetailDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear proveedor");
                return new ApiResponseDto<SupplierDetailDto>
                {
                    Success = false,
                    Message = "Error al crear proveedor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<SupplierDetailDto>> UpdateSupplierAsync(int supplierId, SupplierUpdateDto updateDto)
        {
            try
            {
                _logger.LogInformation("Actualizando proveedor {SupplierId}", supplierId);

                var supplier = await _unitOfWork.Suppliers.GetByIdAsync(supplierId);
                if (supplier == null)
                    throw new NotFoundException($"Proveedor con ID {supplierId} no encontrado");

                supplier.Name = updateDto.Name;
                supplier.Contact = updateDto.Contact;
                supplier.Phone = updateDto.Phone;
                supplier.Address = updateDto.Address;

                await _unitOfWork.Suppliers.UpdateAsync(supplier);
                await _unitOfWork.SaveChangesAsync();

                var supplierDetailDto = _mapper.Map<SupplierDetailDto>(supplier);

                return new ApiResponseDto<SupplierDetailDto>
                {
                    Success = true,
                    Message = "Proveedor actualizado exitosamente",
                    Data = supplierDetailDto
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar proveedor {SupplierId}", supplierId);
                return new ApiResponseDto<SupplierDetailDto>
                {
                    Success = false,
                    Message = "Error al actualizar proveedor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteSupplierAsync(int supplierId)
        {
            try
            {
                _logger.LogInformation("Eliminando proveedor {SupplierId}", supplierId);

                var supplier = await _unitOfWork.Suppliers.GetSupplierWithProductsAsync(supplierId);
                if (supplier == null)
                    throw new NotFoundException($"Proveedor con ID {supplierId} no encontrado");

                if (supplier.ProductSuppliers.Any())
                    throw new InvalidOperationException("No se puede eliminar un proveedor que tiene productos asignados");

                var result = await _unitOfWork.Suppliers.DeleteAsync(supplierId);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = "Proveedor eliminado exitosamente",
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
                _logger.LogError(ex, "Error al eliminar proveedor {SupplierId}", supplierId);
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al eliminar proveedor",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
