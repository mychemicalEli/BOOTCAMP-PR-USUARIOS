using framework.Application;

namespace framework.Infrastructure.Rest;

//Estructura que vamos a devolver en una respuesta paginada
public class PagedResponse<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public PagedList<T>? Data { get; set; }
}