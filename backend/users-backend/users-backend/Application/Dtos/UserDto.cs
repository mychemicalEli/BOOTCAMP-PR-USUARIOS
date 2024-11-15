using framework.Application.Dtos;

namespace users_backend.Application.Dtos;

public class UserDto:IDto
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string Email { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
}