namespace framework.Application;

//Configuración de los parámetros de entrada para implementar paginación
public class PaginationParameters
{
    const int maxPageSize = 50;

    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }

    public string? Sort { get; set; }
}