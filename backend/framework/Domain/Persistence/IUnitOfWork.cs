namespace framework.Domain.Persistence;

//Interfaz genérico de Unit of work
public interface IUnitOfWork
{
    IWork Init();
}