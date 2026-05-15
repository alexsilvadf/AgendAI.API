namespace AgendAI.Domain.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException(string message)
        : base(message, 403, "Forbidden")
    {
    }

    public ForbiddenException()
        : base("You do not have permission to perform this action.", 403, "Forbidden")
    {
    }
}
