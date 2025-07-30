using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;

namespace Marketplace.API.Mapping;

public class RoleProfile: Profile
{
    public RoleProfile() 
    {
        CreateMap<RoleCreateDto, Role>();
    }
}
