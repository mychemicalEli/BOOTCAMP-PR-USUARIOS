using AutoMapper;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;

namespace users_backend.Application.Mappings;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}