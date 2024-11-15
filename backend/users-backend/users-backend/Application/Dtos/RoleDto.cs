using framework.Application.Dtos;

namespace users_backend.Application.Dtos;

public class RoleDto:IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}