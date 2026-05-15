namespace AgendAI.Domain.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message)
        : base(message, 404, "Not Found")
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.", 404, "Not Found")
    {
    }
}
