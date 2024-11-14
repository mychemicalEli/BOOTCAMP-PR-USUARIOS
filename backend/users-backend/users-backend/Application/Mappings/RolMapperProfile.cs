using AutoMapper;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;

namespace users_backend.Application.Mappings;

public class RolMapperProfile:Profile
{
    public RolMapperProfile()
    {
        CreateMap<Rol, RolDto>();
        CreateMap<RolDto, Rol>();
    }
}