using users_backend.Domain.Entities;

namespace users_backend.Infrastructure.Persistence;

public class DataLoader
{
    private readonly UsersContext usersContext;

    public DataLoader(UsersContext usersContext)
    {
        this.usersContext = usersContext;
    }

    public void LoadData()
    {
        if (!usersContext.Roles.Any())
        {
            LoadRoles();
        }

        usersContext.SaveChanges();
        if (!usersContext.Users.Any())
        {
            LoadUsers();
        }

        usersContext.SaveChanges();
    }

    public void LoadRoles()
    {
        var roles = new Role[]
        {
            new Role { Name = "Administrador" },
            new Role { Name = "Contribuidor" },
        };
        foreach (Role rol in roles)
        {
            usersContext.Roles.Add(rol);
        }
    }

    public void LoadUsers()
    {
        var users = new User[]
        {
            new User { Name = "Elizabeth", LastName = "Blanco Méndez", Email = "elizabeth.blanco@gmail.com", RoleId = 1 },
            new User { Name = "Ángel", LastName = "Hernández Castejón", Email = "angel.hernandez@hotmail.com", RoleId = 1 },
            new User { Name = "Carlos", LastName = "Sánchez Rodríguez", Email = "carlos.sanchez@yahoo.com", RoleId = 2 },
            new User { Name = "Ana", LastName = "Martínez López", Email = "ana.martinez@gmail.com", RoleId = 2 },
            new User { Name = "Luis", LastName = "González Fernández", Email = "luis.gonzalez@outlook.com", RoleId = 2 },
            new User { Name = "Marta", LastName = "Jiménez García", Email = "marta.jimenez@icloud.com", RoleId = 2 },
            new User { Name = "Pedro", LastName = "Ramírez Martínez", Email = "pedro.ramirez@live.com", RoleId = 2 },
            new User { Name = "Juan", LastName = "Pérez Gómez", Email = "juan.perez@gmail.com", RoleId = 2 },
            new User { Name = "Ana", LastName = "García López", Email = "ana.garcia@yahoo.com", RoleId = 2 },
            new User { Name = "Carlos", LastName = "Martínez Rodríguez", Email = "carlos.martinez@hotmail.com", RoleId = 1 },
            new User { Name = "María", LastName = "Fernández García", Email = "maria.fernandez@gmail.com", RoleId = 2 },
            new User { Name = "David", LastName = "Lopez Sánchez", Email = "david.lopez@outlook.com", RoleId = 1 },
            new User { Name = "Laura", LastName = "González Ruiz", Email = "laura.gonzalez@icloud.com", RoleId = 2 },
            new User { Name = "Marta", LastName = "Jiménez Díaz", Email = "marta.jimenez@hotmail.com", RoleId = 2 },
            new User { Name = "Pedro", LastName = "Álvarez Martínez", Email = "pedro.alvarez@aol.com", RoleId = 1 },
            new User { Name = "Isabel", LastName = "Sánchez Pérez", Email = "isabel.sanchez@gmail.com", RoleId = 2 },
            new User { Name = "Fernando", LastName = "Ramírez Torres", Email = "fernando.ramirez@outlook.com", RoleId = 2 }

        };
        foreach (User user in users)
        {
            usersContext.Users.Add(user);
        }
    }
}