using AutoMapper;
using Marketplace.Domain.Entities;
using Marketplace.API.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Marketplace.API.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
    }
}