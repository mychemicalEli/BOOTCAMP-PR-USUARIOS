using System.Linq.Expressions;
using System.Reflection;
using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace framework.Infrastructure.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual List<T> GetAll()
    {
        return _dbSet.ToList<T>();
    }

    public virtual T GetById(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            throw new ElementNotFoundException();
        }

        return entity;
    }

    public virtual T Insert(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public virtual T Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public virtual void Delete(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
            throw new ElementNotFoundException();
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
    

    // Este método construye la expresion LINQ que establece la ordenación de la consulta
    protected virtual IQueryable<T> ApplySortOrder(IQueryable<T> entities, string sortOrder)
    {
        // Divide el parámetro `sortOrder` en partes separadas por coma.
        // La primera parte (orderByParameters[0]) es el atributo por el que se quiere ordenar,
        // y la segunda parte es la dirección ("asc" o "desc").
        var orderByParameters = sortOrder.Split(',');

        // Convierte la primera letra del atributo a mayúscula para que coincida con el nombre de la propiedad en `T`.
        var orderByAttribute = Char.ToUpper(orderByParameters[0][0]) + orderByParameters[0][1..];

        // Si `sortOrder` contiene una segunda parte, la usa como dirección de ordenación; si no, usa "asc" por defecto.
        var orderByDirection = orderByParameters.Length > 1 ? orderByParameters[1] : "asc";

        // Obtiene la información de la propiedad en `T` usando reflexión, buscando una propiedad que coincida con `orderByAttribute`.
        var propertyInfo = typeof(T).GetProperty(orderByAttribute,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        // Si se encontró la propiedad, se procede a construir la expresión de ordenación.
        if (propertyInfo != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Obtiene una referencia a la propiedad en `parameter` usando la información de `propertyInfo`.
            var property = Expression.Property(parameter, propertyInfo);

            // Si la propiedad es de tipo valor (por ejemplo, `int`, `double`, etc.), se requiere una conversión a `object`.
            if (propertyInfo.PropertyType.IsValueType)
            {
                // Crea una expresión lambda que convierte el valor de la propiedad a `object` para la ordenación dinámica.
                var orderByExpression =
                    Expression.Lambda<Func<T, dynamic>>(Expression.Convert(property, typeof(object)), parameter);

                // Aplica la ordenación: usa `OrderBy` para ascendente o `OrderByDescending` para descendente.
                entities = orderByDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? entities.OrderBy(orderByExpression)
                    : entities.OrderByDescending(orderByExpression);
            }
            else
            {
                // Si la propiedad no es de tipo valor (por ejemplo, es `string`), se puede usar la propiedad directamente.
                var orderByExpression = Expression.Lambda<Func<T, object>>(property, parameter);

                // Aplica la ordenación: usa `OrderBy` para ascendente o `OrderByDescending` para descendente.
                entities = orderByDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? entities.OrderBy(orderByExpression)
                    : entities.OrderByDescending(orderByExpression);
            }
        }

        // Devuelve la colección `entities` ordenada según el atributo y la dirección especificados en `sortOrder`.
        return entities;
    }
}