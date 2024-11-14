using framework.Application.Dtos;

namespace users_backend.Application.Dtos;

public class UserDto:IDto
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public long RolId { get; set; }
    public string RolName { get; set; }
}