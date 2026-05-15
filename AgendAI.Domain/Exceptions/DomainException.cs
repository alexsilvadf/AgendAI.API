namespace AgendAI.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message, int statusCode, string title)
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    public int StatusCode { get; }

    public string Title { get; }
}
