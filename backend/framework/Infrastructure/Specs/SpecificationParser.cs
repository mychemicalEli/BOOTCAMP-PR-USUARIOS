using framework.Application;

namespace framework.Infrastructure.Specs;

public class SpecificationParser<T> : ISpecificationParser<T>
    where T : class
{
    public Specification<T> ParseSpecification(string filter) 
    {
        List<Criterion> criteria = new List<Criterion>();
        
        //Ejemplo de filtro que entra como cadena => "category.name:match:cha,id:greater_than:10"

        //Descomponemos cada uno de los predicados separados por coma
        var criteriaStrings = filter.Split(',');
        foreach (var criterionString in criteriaStrings)
        {
            //Separamos cada uno de los criterios (el nombre, la operación y la constante)
            //Serían category.name / match / cha
            var parts = criterionString.Split(':');
            if (parts.Length != 3)
            {
                throw new MalformedFilterException();
            }

            //Si va bien creamos un objeto de tipo criterio que contiene estos tres campos (field, operator, value)
            var criterion = new Criterion
            {
                //En lugar de poner directamente en Field parts[0] para que convierta a mayúscula la primera
                //letra de cada propiedad por si le llega en minúscula o si no no encontraría la propiedad
                Field = Char.ToUpper(parts[0][0]) + parts[0][1..],
                Operator = parts[1].ToUpper(),
                Value = parts[2]
            };
            criteria.Add(criterion);
        }

        //Creamos objeto Specification con los criterios (criteria)
        return new Specification<T>(criteria);
    }
}