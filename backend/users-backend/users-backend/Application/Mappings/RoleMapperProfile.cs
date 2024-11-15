using AutoMapper;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;

namespace users_backend.Application.Mappings;

public class RoleMapperProfile:Profile
{
    public RoleMapperProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<RoleDto, Role>();
    }
}