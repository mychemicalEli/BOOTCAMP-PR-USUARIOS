using AutoMapper;
using framework.Application;
using framework.Application.Services;
using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;
using users_backend.Infrastructure.Persistence;

namespace users_backend.Application.Services;

public class UserService : GenericService<User, UserDto>, IUserService
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

    public override UserDto Update(UserDto userDto)
    {
        var user = _userRepository.GetById(userDto.Id);

        if (user == null)
        {
            throw new ElementNotFoundException();
        }

        if (userDto.RowVersion != user.RowVersion)
        {
            throw new ConcurrencyException("The user record has been modified by another admin.");
        }

        try
        {
            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.RoleId = userDto.RoleId;

            _userRepository.Update(user);
            return _mapper.Map<UserDto>(user);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(
                "El usuario ya fue actualizado por otra persona. Actualiza la información antes de continuar.", ex);
        }
    }
}