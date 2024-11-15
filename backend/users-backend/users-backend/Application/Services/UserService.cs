using AutoMapper;
using framework.Application;
using framework.Application.Services;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;

namespace users_backend.Application.Services;

public class UserService :GenericService<User, UserDto>, IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
    }
    
    public PagedList<UserDto> GetUsersByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
    {
        var users = _userRepository.GetUsersByCriteriaPaged(filter, paginationParameters);
        return users;
    }
}