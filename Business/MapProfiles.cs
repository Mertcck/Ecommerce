using AutoMapper;
using Core.Concretes.DTOs.Product;
using Core.Concretes.DTOs.Sales;
using Core.Concretes.Entities;

namespace Business
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            // CreateDto -> Entity
            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<CreateProductImageDto, ProductImage>();
            CreateMap<CreateProductFeatureDto, ProductFeature>();

            // Entity -> ListDto
            CreateMap<Product, StoreProductListDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Category, CategoryListDto>();
            CreateMap<Brand, BrandListDto>();

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductFeature, ProductFeatureDto>();

            CreateMap<Product, ProductCardDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.Stock > 0))
                .ForMember(dest => dest.CoverImagePath, opt => opt.MapFrom(src => src.Images.Any() ? src.Images.OrderBy(x => x.DisplayOrder).FirstOrDefault()!.ImageUrl : null));

            // Entity <-> UpdateDto
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Brand, UpdateBrandDto>().ReverseMap();

            // Entity -> DetailDto
            CreateMap<Product, StoreProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
