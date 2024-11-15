using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace users_backend.Infrastructure.Persistence;

public class RoleRepository:GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(UsersContext context) : base(context) {}
}