using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;

namespace Marketplace.API.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Product.Seller.Username));
    }
}
