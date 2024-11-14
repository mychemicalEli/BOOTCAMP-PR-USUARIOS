using System.Linq.Expressions;
using framework.Application;
namespace framework.Infrastructure.Specs;

public class Specification<T>
{
    public List<Criterion> _criteria;

    public Specification(List<Criterion> criteria)
    {
        _criteria = criteria;
    }

    public IQueryable<T> ApplySpecification(IQueryable<T> query)
    {
        #region Qué es una expresión Lambda

        // Una expresión Lamba en .NET Core (y en general en C# y otros lenguajes de programación) es una forma concisa de
        // definir una función anónima que puedes usar para crear delegados o expresiones de árbol de expresiones
        // 
        // Las expresiones Lambda son esencialmente útiles para escribir LINQ para manipular datos en C#. Un ejemplo
        // sencillo de una expresión lambda es el siguiente:
        //
        //      (x, y) => x * y
        //
        // Aquí (x, y) son los parámetros de la función y x * y es el cuerpo de la función. Esto define una función que
        // toma dos parámetros y devuelve su producto.
        //
        // La función equivalente sin utilizar una expresión lambda sería:
        //
        //      public multiply(int x, int y){
        //          return x * y
        //      }
        //
        // Las expresiones lambda pueden ser usadas en muchos contexto. Uno de los usos más comunes es en consultas LINQ,
        // donde puedes usarlas para definir predicados y proyecciones.
        //
        // Por ejemplo, en el siguiente código se usa una expresión lambda para definir un filtro en una consulta LINQ:
        //
        //      var adults = people.Where(p => p.Age >= 18);
        //
        // Aquí p => p.Age >= 18 es una expresión lambda que define un predicado. Este predicado se usa para filtrar la
        // lista de personas y seleccionar solo aquellas que tienen 18 años o más.

        #endregion

        #region Representación de una variable en una expresión lambda

        // En la siguiente línea, con Expression.Parameter(typeof(T), "x"), estamos creando un objeto de tipo ParameterExpression.
        // Un ParameterExpression se utiliza para representar una variable en una expresión lambda.
        //
        // En este caso, el código Expression.Parameter(typeof(T), "x") crea un parámetro llamado "x" que es del tipo T.
        // Este parámetro luego se utiliza para construir la expresión lambda que se pasa al método Where.
        //
        // Por ejemplo, si estamos buscando elementos de tipo Item cuyo campo Price es mayor a un valor dado, la expresión lambda
        // generada sería algo así:
        //
        //      x => x.Price > value

        #endregion

        var parameter = Expression.Parameter(typeof(T), "x");

        #region

        // La clase System.Linq.Expressions.Expression en .NET Core es una clase fundamental que representa una expresión
        // en un árbol de expresiones. Un árbol de expresiones es una estructura de datos que representa algún código
        // computacional en la forma de una estructura de árbol en la que cada nodo es una expresión, por ejemplo, un método
        // de llamada, un operador binario, una referencia a un parámetro, etc.
        //
        // Por ejemplo, si nos llegase un filtro como este "Price:GREATER_THAN:10,id:GREATER_THAN:50", el código que viene a
        // continuación generaría un árbol como el siguiente:
        //
        // Lambda
        //  Parameter
        //      x
        // AndAlso
        //  GreaterThan
        //      MemberAccess
        //          Price
        //      Constant
        //          10
        //  GreaterThan
        //      MemberAccess
        //          Id
        //      Constant
        //          50

        #endregion

        foreach (var criterion in _criteria)
        {
            var criterionFields = criterion.Field.Split('.');
            var property = GetProperty(parameter, criterionFields);
            var constant = Expression.Constant(Convert.ChangeType(criterion.Value, property.Type));

            Expression? predicate = null;

            switch (criterion.Operator)
            {
                case "EQUAL":
                    predicate = Expression.Equal(property, constant);
                    break;
                case "GREATER_THAN":
                    predicate = Expression.GreaterThan(property, constant);
                    break;
                case "LESS_THAN":
                    predicate = Expression.LessThan(property, constant);
                    break;
                case "GREATER_THAN_EQUAL":
                    predicate = Expression.GreaterThanOrEqual(property, constant);
                    break;
                case "LESS_THAN_EQUAL":
                    predicate = Expression.LessThanOrEqual(property, constant);
                    break;
                case "NOT_EQUAL":
                    predicate = Expression.NotEqual(property, constant);
                    break;
                case "MATCH":
                    predicate = Expression.Call(ToCaseInsensitiveStringExpression(property), "Contains", null,
                        ToCaseInsensitiveStringExpression(
                            constant));
                    break;
                case "MATCH_START":
                    predicate = Expression.Call(ToCaseInsensitiveStringExpression(property), "StartsWith", null,
                        ToCaseInsensitiveStringExpression(
                            constant));
                    break;
                case "MATCH_END":
                    predicate = Expression.Call(ToCaseInsensitiveStringExpression(property), "EndsWith", null,
                        ToCaseInsensitiveStringExpression(
                            constant));
                    break;
                default:
                    throw new MalformedFilterException();
            }

            if (predicate != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                query = query.Where(lambda);
            }
        }

        return query;
    }

    // Esta función la necesitamos para implementar los operadores MATCH, MATCH_START Y MATCH_END, ya que
    // estos operadores trabajan con strings y nosotros podemos recibir filtros asociados a campos numéricos
    // Además, hace que dé igual que le pasemos los atributos con mayúsculas o minúsculas, es case insensitive
    private Expression ToCaseInsensitiveStringExpression(Expression expression)
    {
        if (expression.Type != typeof(string))
        {
            var toStringMethod = expression.Type.GetMethod("ToString", Array.Empty<Type>()) ??
                                 throw new MalformedFilterException();
            expression = Expression.Call(expression, toStringMethod);
        }

        var toLowerMethod = typeof(string).GetMethod("ToLower", Array.Empty<Type>()) ??
                            throw new MalformedFilterException();
        expression = Expression.Call(expression, toLowerMethod);
        return expression;
    }

    // Esta función la necesitamos ya que en los filtros se puede hacer referencia a propiedades asociadas
    // a entidades anidadas, como por ejemplo item.Category.Name
    private MemberExpression GetProperty(Expression instance, string[] propertyNames, int index = 0)
    {
        var propertyName = Char.ToUpper(propertyNames[index][0]) + propertyNames[index][1..];
        var property = Expression.Property(instance, propertyName);
        if (index + 1 == propertyNames.Length)
        {
            return property;
        }

        return GetProperty(property, propertyNames, index + 1);
    }
}