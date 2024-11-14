using framework.Application.Dtos;

namespace users_backend.Application.Dtos;

public class RolDto:IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}