using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace users_backend.Infrastructure.Persistence;

public class RolRepository:GenericRepository<Rol>, IRolRepository
{
    public RolRepository(UsersContext context) : base(context) {}
    
}