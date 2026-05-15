namespace AgendAI.Domain.Exceptions;

public sealed class ConflictException : DomainException
{
    public ConflictException(string message)
        : base(message, 409, "Conflict")
    {
    }
}
