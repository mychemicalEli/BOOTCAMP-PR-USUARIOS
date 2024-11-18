using System.Runtime.Serialization;

namespace framework.Domain.Persistence;

[Serializable]
public class ConcurrencyException : Exception
{
    public ConcurrencyException()
    {
    }

    public ConcurrencyException(string? message) : base(message)
    {
    }

    public ConcurrencyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}