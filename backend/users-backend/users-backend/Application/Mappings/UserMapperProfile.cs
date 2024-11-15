using AutoMapper;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;

namespace users_backend.Application.Mappings;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Rol.Name));

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Rol, opt => opt.Ignore());
    }
}