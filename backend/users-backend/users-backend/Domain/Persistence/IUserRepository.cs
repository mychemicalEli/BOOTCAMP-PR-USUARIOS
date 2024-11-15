using framework.Application;
using framework.Domain.Persistence;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;

namespace users_backend.Domain.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    PagedList<UserDto> GetUsersByCriteriaPaged(string? filter, PaginationParameters paginationParameters);
}