namespace AgendAI.Domain.Exceptions;

public sealed class ValidationException : DomainException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", 400, "Validation Error")
    {
        Errors = errors;
    }

    public ValidationException(string field, string message)
        : this(new Dictionary<string, string[]>
        {
            [field] = [message]
        })
    {
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
