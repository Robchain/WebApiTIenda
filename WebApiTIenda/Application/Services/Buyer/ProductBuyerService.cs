using WebApiTIenda.Application.DTOs.Admin;
using WebApiTIenda.Application.DTOs.Buyer;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Domian.Interfaces;
using AutoMapper;

namespace WebApiTIenda.Application.Services.Buyer
{
    public class ProductBuyerService : IProductBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductBuyerService> _logger;

        public ProductBuyerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductBuyerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<DTOs.Buyer.ProductListDto>>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos para compradores");

                var products = await _unitOfWork.Products.GetAllWithSuppliersAsync();

                var productDtos = products.Select(p => new DTOs.Buyer.ProductListDto
                {
                    IdProduct = p.IdProduct,
                    Name = p.Name,
                    Description = p.Description,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    CreationDate = p.CreationDate,
                    BestPrice = p.ProductSuppliers.Any() ? p.ProductSuppliers.Min(ps => ps.Price) : 0,
                    IsAvailable = p.ProductSuppliers.Any(ps => ps.Stock > 0),
                    TotalStock = p.ProductSuppliers.Sum(ps => ps.Stock)
                }).ToList();

                return new ApiResponseDto<List<DTOs.Buyer.ProductListDto>>
                {
                    Success = true,
                    Message = "Productos obtenidos exitosamente",
                    Data = productDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return new ApiResponseDto<List<DTOs.Buyer.ProductListDto>>
                {
                    Success = false,
                    Message = "Error al obtener productos",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public Task<ApiResponseDto<DTOs.Buyer.ProductListDto>> GetProductWithPricesAsync(int productId, int? userId = null)
        {
            throw new NotImplementedException();
        }
    }
}
