using users_backend.Application.Dtos;
using users_backend.Application.Services;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Mvc;

namespace users_backend.Infrastructure.Rest;

[Route("/users/[controller]")]
[ApiController]
public class RolesController : GenericCrudController<RoleDto>
{

    public RolesController(IRoleService roleService): base(roleService)
    {
    }
}