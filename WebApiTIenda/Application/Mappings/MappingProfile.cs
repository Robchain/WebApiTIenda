using AutoMapper;
using WebApiTIenda.Domian.Entities;
using WebApiTIenda.Shared.Helpers;
using WebApiTIenda.Application.DTOs.Buyer;
using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============== PRODUCT MAPPINGS ==============

            // Product -> Buyer.ProductListDto
            CreateMap<Product, DTOs.Buyer.ProductListDto>()
                .ForMember(dest => dest.BestPrice, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any() ? src.ProductSuppliers.Min(ps => ps.Price) : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any(ps => ps.Stock > 0)))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Sum(ps => ps.Stock)));

            // Product -> Buyer.ProductDetailDto
            CreateMap<Product, DTOs.Buyer.ProductDetailDto>()
                .ForMember(dest => dest.BestPrice, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any() ? src.ProductSuppliers.Min(ps => ps.Price) : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any(ps => ps.Stock > 0)))
                .ForMember(dest => dest.PriceOptions, opt => opt.MapFrom(src => src.ProductSuppliers))
                .ForMember(dest => dest.IsInWishList, opt => opt.Ignore());

            // Product -> Admin.ProductListDto
            CreateMap<Product, DTOs.Admin.ProductListDto>()
                .ForMember(dest => dest.TotalSuppliers, opt => opt.MapFrom(src => src.ProductSuppliers.Count))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(src => src.ProductSuppliers.Sum(ps => ps.Stock)))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any() ? src.ProductSuppliers.Min(ps => ps.Price) : 0))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any() ? src.ProductSuppliers.Max(ps => ps.Price) : 0))
                .ForMember(dest => dest.HasActiveSuppliers, opt => opt.MapFrom(src =>
                    src.ProductSuppliers.Any(ps => ps.Stock > 0)));

            // Product -> Admin.ProductDetailDto
            CreateMap<Product, DTOs.Admin.ProductDetailDto>()
                .ForMember(dest => dest.Suppliers, opt => opt.MapFrom(src => src.ProductSuppliers));

            // Admin.ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.IdProduct, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore())
                .ForMember(dest => dest.WishListDetails, opt => opt.Ignore())
                .ForMember(dest => dest.OrderRDetails, opt => opt.Ignore());

            // Admin.ProductUpdateDto -> Product
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.IdProduct, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore())
                .ForMember(dest => dest.WishListDetails, opt => opt.Ignore())
                .ForMember(dest => dest.OrderRDetails, opt => opt.Ignore());

            // ============== SUPPLIER MAPPINGS ==============

            // Supplier -> Admin.SupplierListDto
            CreateMap<Supplier, SupplierListDto>()
                .ForMember(dest => dest.IdSupplier, opt => opt.MapFrom(src => src.IdSuPlier))
                .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.ProductSuppliers.Count))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ProductSuppliers.Any(ps => ps.Stock > 0)));

            // Supplier -> Admin.SupplierDetailDto
            CreateMap<Supplier, SupplierDetailDto>()
                .ForMember(dest => dest.IdSupplier, opt => opt.MapFrom(src => src.IdSuPlier))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductSuppliers))
                .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.ProductSuppliers.Count));

            // Admin.SupplierCreateDto -> Supplier
            CreateMap<SupplierCreateDto, Supplier>()
                .ForMember(dest => dest.IdSuPlier, opt => opt.Ignore())
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore());

            // Admin.SupplierUpdateDto -> Supplier
            CreateMap<SupplierUpdateDto, Supplier>()
                .ForMember(dest => dest.IdSuPlier, opt => opt.Ignore())
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore());

            // ============== PRODUCT SUPPLIER MAPPINGS ==============

            // ProductSupplier -> Buyer.ProductPriceOptionDto
            CreateMap<ProductSupplier, ProductPriceOptionDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
                .ForMember(dest => dest.LotCode, opt => opt.MapFrom(src => src.Lot.CodeLote))
                .ForMember(dest => dest.LotExpDate, opt => opt.MapFrom(src => src.Lot.ExpDate));

            // ProductSupplier -> Admin.ProductSupplierDetailDto
            CreateMap<ProductSupplier, ProductSupplierDetailDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
                .ForMember(dest => dest.SupplierContact, opt => opt.MapFrom(src => src.Supplier.Contact))
                .ForMember(dest => dest.SupplierPhone, opt => opt.MapFrom(src => src.Supplier.Phone))
                .ForMember(dest => dest.LotCode, opt => opt.MapFrom(src => src.Lot.CodeLote))
                .ForMember(dest => dest.LotExpDate, opt => opt.MapFrom(src => src.Lot.ExpDate))
                .ForMember(dest => dest.IsLotExpired, opt => opt.MapFrom(src => src.Lot.ExpDate < DateTime.Now))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.Now));

            // ProductSupplier -> Admin.SupplierProductDto
            CreateMap<ProductSupplier, SupplierProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.LotCode, opt => opt.MapFrom(src => src.Lot.CodeLote))
                .ForMember(dest => dest.LotExpDate, opt => opt.MapFrom(src => src.Lot.ExpDate))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.Lot.ExpDate < DateTime.Now));

            // Admin.ProductSupplierCreateDto -> ProductSupplier
            CreateMap<ProductSupplierCreateDto, ProductSupplier>();

            // Admin.ProductSupplierUpdateDto -> ProductSupplier
            CreateMap<ProductSupplierUpdateDto, ProductSupplier>()
                .ForMember(dest => dest.IdProduct, opt => opt.Ignore())
                .ForMember(dest => dest.IdSupplier, opt => opt.Ignore())
                .ForMember(dest => dest.IdLot, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Lot, opt => opt.Ignore());

            // ============== LOT MAPPINGS ==============

            // Lot -> Admin.LotListDto
            CreateMap<Lot, LotListDto>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpDate < DateTime.Now))
                .ForMember(dest => dest.DaysUntilExpiration, opt => opt.MapFrom(src => (src.ExpDate - DateTime.Now).Days))
                .ForMember(dest => dest.ExpirationStatus, opt => opt.MapFrom(src =>
                    src.ExpDate < DateTime.Now ? "Vencido" :
                    (src.ExpDate - DateTime.Now).Days <= 30 ? "Próximo a vencer" : "Vigente"))
                .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.ProductSuppliers.Count));

            // Lot -> Admin.LotDetailDto
            CreateMap<Lot, LotDetailDto>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpDate < DateTime.Now))
                .ForMember(dest => dest.DaysUntilExpiration, opt => opt.MapFrom(src => (src.ExpDate - DateTime.Now).Days))
                .ForMember(dest => dest.ExpirationStatus, opt => opt.MapFrom(src =>
                    src.ExpDate < DateTime.Now ? "Vencido" :
                    (src.ExpDate - DateTime.Now).Days <= 30 ? "Próximo a vencer" : "Vigente"))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductSuppliers));

            // ProductSupplier -> Admin.LotProductDto (para mostrar productos en un lote)
            CreateMap<ProductSupplier, LotProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

            // Admin.LotCreateDto -> Lot
            CreateMap<LotCreateDto, Lot>()
                .ForMember(dest => dest.IdLot, opt => opt.Ignore())
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore());

            // Admin.LotUpdateDto -> Lot
            CreateMap<LotUpdateDto, Lot>()
                .ForMember(dest => dest.IdLot, opt => opt.Ignore())
                .ForMember(dest => dest.ProductSuppliers, opt => opt.Ignore());

            // ============== WISHLIST MAPPINGS ==============

            // WishListDetail -> Buyer.WishListItemDto
            CreateMap<WishListDetail, WishListItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.Product.Category))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.BestPrice, opt => opt.MapFrom(src =>
                    src.Product.ProductSuppliers.Any() ? src.Product.ProductSuppliers.Min(ps => ps.Price) : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                    src.Product.ProductSuppliers.Any(ps => ps.Stock > 0)));

            // ============== CART MAPPINGS ==============

            // CartItem -> Buyer.CartItemDto (ya tienes la clase CartItem personalizada)
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductImageUrl, opt => opt.Ignore()); // Se puede agregar después

            // Buyer.AddToCartDto -> CartItem
            CreateMap<AddToCartDto, CartItem>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.IdProduct))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.IdSupplier))
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.ProductName, opt => opt.Ignore())
                .ForMember(dest => dest.SupplierName, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableStock, opt => opt.Ignore());
        }
    }
}
