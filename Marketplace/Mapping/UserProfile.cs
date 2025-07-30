using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;

namespace Marketplace.API.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(
                src => src.UserRoles.Select(ur => ur.Role.Name).ToList()));
    }
}
