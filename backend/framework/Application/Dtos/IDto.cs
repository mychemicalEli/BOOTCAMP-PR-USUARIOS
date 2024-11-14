namespace framework.Application.Dtos;

//Exige que todos los DTOs tengan un ID
public interface IDto
{
    public long Id { get; set; }
}