using framework.Application;
using framework.Application.Services;
using users_backend.Application.Dtos;

namespace users_backend.Application.Services;

public interface IUserService : IGenericService<UserDto>
{
    PagedList<UserDto> GetUsersByCriteriaPaged(string? filter, PaginationParameters paginationParameters);
}