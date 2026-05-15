using System.Reflection;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

public static class EnumExtensions
{
    public static string ToJsonValue<TEnum>(this TEnum value)
        where TEnum : struct, Enum
    {
        var field = typeof(TEnum).GetField(value.ToString()!);
        if (field is null)
            return value.ToString()!.ToLowerInvariant();

        return field.GetCustomAttribute<EnumJsonValueAttribute>()?.Value
            ?? SnakeCaseLowerJsonNamingPolicy.Instance.ConvertName(field.Name);
    }

    public static TEnum FromJsonValue<TEnum>(string value)
        where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var jsonValue = field.GetCustomAttribute<EnumJsonValueAttribute>()?.Value
                ?? SnakeCaseLowerJsonNamingPolicy.Instance.ConvertName(field.Name);

            if (string.Equals(jsonValue, value, StringComparison.OrdinalIgnoreCase))
                return (TEnum)field.GetValue(null)!;
        }

        throw new ArgumentException($"Valor '{value}' inválido para enum {typeof(TEnum).Name}.");
    }
}
