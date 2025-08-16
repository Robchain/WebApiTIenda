using AutoMapper;
using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Domian.Exceptions;
using WebApiTIenda.Domian.Interfaces;

namespace WebApiTIenda.Application.Services.Admin
{
    public class ProductAdminService : IProductAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductAdminService> _logger;

        public ProductAdminService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<ProductListDto>>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos para administradores");

                var products = await _unitOfWork.Products.GetAllWithSuppliersAsync();

                var productDtos = products.Select(p => new ProductListDto
                {
                    IdProduct = p.IdProduct,
                    Name = p.Name,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    CreationDate = p.CreationDate,
                    TotalSuppliers = p.ProductSuppliers.Count,
                    TotalStock = p.ProductSuppliers.Sum(ps => ps.Stock),
                    MinPrice = p.ProductSuppliers.Any() ? p.ProductSuppliers.Min(ps => ps.Price) : 0,
                    MaxPrice = p.ProductSuppliers.Any() ? p.ProductSuppliers.Max(ps => ps.Price) : 0,
                    HasActiveSuppliers = p.ProductSuppliers.Any(ps => ps.Stock > 0)
                }).ToList();

                return new ApiResponseDto<List<ProductListDto>>
                {
                    Success = true,
                    Message = "Productos obtenidos exitosamente",
                    Data = productDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos para administradores");
                return new ApiResponseDto<List<ProductListDto>>
                {
                    Success = false,
                    Message = "Error al obtener productos",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<ProductDetailDto>> GetProductDetailAsync(int productId)
        {
            try
            {
                _logger.LogInformation("Obteniendo detalles del producto {ProductId} para administradores", productId);

                var product = await _unitOfWork.Products.GetProductWithSuppliersAsync(productId);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {productId} no encontrado");

                var productDetailDto = new ProductDetailDto
                {
                    IdProduct = product.IdProduct,
                    Name = product.Name,
                    Description = product.Description,
                    Category = product.Category,
                    ImageUrl = product.ImageUrl,
                    CreationDate = product.CreationDate,
                    Suppliers = product.ProductSuppliers.Select(ps => new ProductSupplierDetailDto
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
                        LastUpdated = DateTime.Now // En producción vendría de la entidad
                    }).ToList()
                };

                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = true,
                    Message = "Detalles del producto obtenidos exitosamente",
                    Data = productDetailDto
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del producto {ProductId}", productId);
                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error al obtener detalles del producto",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<ProductDetailDto>> CreateProductAsync(ProductCreateDto createDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo producto: {ProductName}", createDto.Name);

                var product = new Product
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    Category = createDto.Category,
                    ImageUrl = createDto.ImageUrl,
                    CreationDate = DateTime.Now
                };

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                var productDetailDto = new ProductDetailDto
                {
                    IdProduct = product.IdProduct,
                    Name = product.Name,
                    Description = product.Description,
                    Category = product.Category,
                    ImageUrl = product.ImageUrl,
                    CreationDate = product.CreationDate,
                    Suppliers = new List<ProductSupplierDetailDto>()
                };

                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = true,
                    Message = "Producto creado exitosamente",
                    Data = productDetailDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error al crear producto",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<ProductDetailDto>> UpdateProductAsync(int productId, ProductUpdateDto updateDto)
        {
            try
            {
                _logger.LogInformation("Actualizando producto {ProductId}", productId);

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {productId} no encontrado");

                product.Name = updateDto.Name;
                product.Description = updateDto.Description;
                product.Category = updateDto.Category;
                product.ImageUrl = updateDto.ImageUrl;

                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                // Obtener producto actualizado con proveedores
                var updatedProduct = await _unitOfWork.Products.GetProductWithSuppliersAsync(productId);
                var productDetailDto = _mapper.Map<ProductDetailDto>(updatedProduct);

                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = true,
                    Message = "Producto actualizado exitosamente",
                    Data = productDetailDto
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {ProductId}", productId);
                return new ApiResponseDto<ProductDetailDto>
                {
                    Success = false,
                    Message = "Error al actualizar producto",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteProductAsync(int productId)
        {
            try
            {
                _logger.LogInformation("Eliminando producto {ProductId}", productId);

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    throw new NotFoundException($"Producto con ID {productId} no encontrado");

                // Verificar si el producto tiene proveedores activos
                var productWithSuppliers = await _unitOfWork.Products.GetProductWithSuppliersAsync(productId);
                if (productWithSuppliers?.ProductSuppliers.Any() == true)
                {
                    throw new InvalidOperationException("No se puede eliminar un producto que tiene proveedores asignados");
                }

                var result = await _unitOfWork.Products.DeleteAsync(productId);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponseDto<bool>
                {
                    Success = result,
                    Message = "Producto eliminado exitosamente",
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
                _logger.LogError(ex, "Error al eliminar producto {ProductId}", productId);
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Error al eliminar producto",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
