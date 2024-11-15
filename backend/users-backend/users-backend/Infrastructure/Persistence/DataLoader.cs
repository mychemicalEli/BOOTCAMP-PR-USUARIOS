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
        var roles = new Rol[]
        {
            new Rol { Name = "Administrator" },
            new Rol { Name = "Contributor" },
        };
        foreach (Rol rol in roles)
        {
            usersContext.Roles.Add(rol);
        }
    }

    public void LoadUsers()
    {
        var users = new User[]
        {
            new User
            {
                Nombre = "Elizabeth", Apellidos = "Blanco Méndez", Email = "elizabeth.blanco@gmail.com", RoleId = 1
            },
            new User
            {
                Nombre = "Ángel", Apellidos = "Hernández Castejón", Email = "angel.hernandez@hotmail.com", RoleId = 1
            },
            new User
            {
                Nombre = "Carlos", Apellidos = "Sánchez Rodríguez", Email = "carlos.sanchez@yahoo.com", RoleId = 2
            },
            new User { Nombre = "Ana", Apellidos = "Martínez López", Email = "ana.martinez@gmail.com", RoleId = 2 },
            new User
            {
                Nombre = "Luis", Apellidos = "González Fernández", Email = "luis.gonzalez@outlook.com", RoleId = 2
            },
            new User { Nombre = "Marta", Apellidos = "Jiménez García", Email = "marta.jimenez@icloud.com", RoleId = 2 },
            new User { Nombre = "Pedro", Apellidos = "Ramírez Martínez", Email = "pedro.ramirez@live.com", RoleId = 2 }
        };
        foreach (User user in users)
        {
            usersContext.Users.Add(user);
        }
    }
}