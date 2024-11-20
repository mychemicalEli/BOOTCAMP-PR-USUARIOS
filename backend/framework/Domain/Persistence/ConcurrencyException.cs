namespace framework.Domain.Persistence;
[Serializable]
public class ConcurrencyException:Exception

{
    public ConcurrencyException() : base("Error de concurrencia.") { }

    public ConcurrencyException(string message) : base(message) { }

    public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
}