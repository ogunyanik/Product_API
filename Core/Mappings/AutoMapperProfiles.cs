using AutoMapper;
using Product_API.Core.DTO;
using Product_API.Core.Models;


namespace Product_API.Core.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    { 
        CreateMap<CategoryDTO, Category>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Product, ProductDTO>();
        CreateMap<ProductDTO, Product>();

    }
}