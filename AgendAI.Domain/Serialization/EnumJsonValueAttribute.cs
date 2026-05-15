namespace AgendAI.Domain.Serialization;

[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumJsonValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}
