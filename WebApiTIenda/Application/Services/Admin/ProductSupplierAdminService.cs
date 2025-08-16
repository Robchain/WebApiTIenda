using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;

namespace WebApiTIenda.Application.Services.Admin
{
    public class ProductSupplierAdminService : IProductSupplierAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductSupplierAdminService> _logger;

        public ProductSupplierAdminService(IUnitOfWork unitOfWork, ILogger<ProductSupplierAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponseDto<bool>> AssignSupplierToProductAsync(ProductSupplierCreateDto createDto)
        {
            try
            {
                _logger.LogInformation("Asignando proveedor {SupplierId} al producto {ProductId}",
                    createDto.IdSupplier, createDto.IdProduct);

                // Verificar que el producto existe
                var product = await _unitOfWork.Products.GetByIdAsync(createDto.IdProduct);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {createDto.IdProduct} no encontrado");

                // Verificar que el proveedor existe
                var supplier = await _unitOfWork.Suppliers.GetByIdAsync(createDto.IdSupplier);
                if (supplier == null)
                    throw new NotFoundException($"Proveedor con ID {createDto.IdSupplier} no encontrado");

                // Verificar que el lote existe
                var lot = await _unitOfWork.Lots.GetByIdAsync(createDto.IdLot);
                if (lot == null)
                    throw new NotFoundException($"Lote con ID {createDto.IdLot} no encontrado");

                // Verificar que no existe ya la relación
                var existingRelation = await _unitOfWork.ProductSuppliers
                    .GetProductSupplierAsync(createDto.IdProduct, createDto.IdSupplier);
                if (existingRelation != null)
                    throw new InvalidOperationException("La relación producto-proveedor ya existe");

                var productSupplier = new ProductSupplier
                {
                    IdProduct = createDto.IdProduct,
                    IdSupplier = createDto.IdSupplier,
                    IdLot = createDto.IdLot,
                    Price = createDto.Price,
                    Stock = createDto.Stock
                };

                await _unitOfWork.ProductSuppliers.AddAsync(productSupplier);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Message = "Proveedor asignado al producto exitosamente",
                    Data = true
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
                _logger.LogError(ex, "Error al asignar proveedor al producto");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al asignar proveedor al producto",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> UpdateProductSupplierAsync(int productId, int supplierId, ProductSupplierUpdateDto updateDto)
        {
            try
            {
                _logger.LogInformation("Actualizando relación producto {ProductId} - proveedor {SupplierId}",
                    productId, supplierId);

                var productSupplier = await _unitOfWork.ProductSuppliers
                    .GetProductSupplierAsync(productId, supplierId);

                if (productSupplier == null)
                    throw new NotFoundException("Relación producto-proveedor no encontrada");

                productSupplier.Price = updateDto.Price;
                productSupplier.Stock = updateDto.Stock;

                await _unitOfWork.ProductSuppliers.UpdateAsync(productSupplier);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Message = "Relación producto-proveedor actualizada exitosamente",
                    Data = true
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar relación producto-proveedor");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al actualizar relación producto-proveedor",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> RemoveSupplierFromProductAsync(int productId, int supplierId)
        {
            try
            {
                _logger.LogInformation("Removiendo proveedor {SupplierId} del producto {ProductId}",
                    supplierId, productId);

                var result = await _unitOfWork.ProductSuppliers.RemoveAsync(productId, supplierId);
                if (result)
                    await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = result ? "Proveedor removido del producto exitosamente" : "Relación no encontrada",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover proveedor del producto");
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al remover proveedor del producto",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<List<ProductSupplierDetailDto>>> GetProductSuppliersAsync(int productId)
        {
            try
            {
                _logger.LogInformation("Obteniendo proveedores del producto {ProductId}", productId);

                var productSuppliers = await _unitOfWork.ProductSuppliers.GetProductSuppliersAsync(productId);

                var supplierDtos = productSuppliers.Select(ps => new ProductSupplierDetailDto
                {
                    IdSupplier = ps.IdSupplier,
                    SupplierName = ps.Supplier.Name,
                    SupplierContact = ps.Supplier.Contact,
                    SupplierPhone = ps.Supplier.Phone,
                    IdLot = ps.IdLot,
                    LotCode = ps.Lot.CodeLote,
                    LotExpDate = ps.Lot.ExpDate,
                    IsLotExpired = ps.Lot.ExpDate < DateTime.Now,
                    Price = ps.Price,
                    Stock = ps.Stock,
                    LastUpdated = DateTime.Now
                }).ToList();

                return new ApiResponseDto<List<ProductSupplierDetailDto>>
                {
                    Success = true,
                    Message = "Proveedores del producto obtenidos exitosamente",
                    Data = supplierDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores del producto {ProductId}", productId);
                return new ApiResponseDto<List<ProductSupplierDetailDto>>
                {
                    Success = false,
                    Message = "Error al obtener proveedores del producto",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
