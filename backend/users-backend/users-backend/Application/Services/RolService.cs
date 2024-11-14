using AutoMapper;
using framework.Application.Services;
using framework.Domain.Persistence;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;

namespace users_backend.Application.Services;

public class RolService : GenericService<Rol, RolDto>, IRolService
{
    public RolService(IRolRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}