using framework.Application;
using framework.Domain.Persistence;
using framework.Infrastructure.Persistence;
using framework.Infrastructure.Specs;
using Microsoft.EntityFrameworkCore;
using users_backend.Application.Dtos;
using users_backend.Domain.Entities;
using users_backend.Domain.Persistence;

namespace users_backend.Infrastructure.Persistence;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private UsersContext _usersContext;
    private readonly ISpecificationParser<User> _specificationParser;

    public UserRepository(UsersContext usersContext, ISpecificationParser<User> specificationParser) : base(
        usersContext)
    {
        _usersContext = usersContext;
        _specificationParser = specificationParser;
    }

    //Sobreescribimos el método GetById para que nos devuelva también el nombre del rol
    public override User GetById(long id)
    {
        var user = _usersContext.Users.Include(i => i.Rol).SingleOrDefault(i => i.Id == id);
        if (user == null)
        {
            throw new ElementNotFoundException();
        }

        return user;
    }

    /* Sobreescribimos el metodo POST y el PUT para que nos devuelvan en el user creado con los datos correspondientes
al rol */
    public override User Insert(User user)
    {
        _usersContext.Users.Add(user);
        _usersContext.SaveChanges();
        _usersContext.Entry(user).Reference(i => i.Rol).Load();
        return user;
    }

    public override User Update(User user)
    {
        _usersContext.Users.Update(user);
        _usersContext.SaveChanges();
        _usersContext.Entry(user).Reference(i => i.Rol).Load();
        return user;
    }


    // Listado paginado y filtrado de usuarios
    public PagedList<UserDto> GetUsersByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
    {
        var users = _usersContext.Users.AsQueryable();
        if (!string.IsNullOrEmpty(filter))
        {
            Specification<User> specification = _specificationParser.ParseSpecification(filter);
            users = specification.ApplySpecification(users);
        }

        if (!string.IsNullOrEmpty(paginationParameters.Sort))
        {
            users = ApplySortOrder(users, paginationParameters.Sort);
        }

        var usersDto = users.Select(i => new UserDto()
        {
            Id = i.Id,
            Nombre = i.Nombre,
            Apellidos = i.Apellidos,
            Email = i.Email,
            RoleId = i.RoleId,
            RoleName = i.Rol.Name
        });
        return PagedList<UserDto>.ToPagedList(usersDto, paginationParameters.PageNumber, paginationParameters.PageSize);
    }
}