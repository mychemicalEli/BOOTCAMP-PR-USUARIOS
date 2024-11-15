using AutoMapper;
using framework.Application.Services;
using framework.Domain.Persistence;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;

namespace users_backend.Application.Services;

public class RoleService : GenericService<Role, RoleDto>, IRoleService
{
    public RoleService(IRoleRepository repository, IMapper mapper) : base(repository, mapper) {}
}