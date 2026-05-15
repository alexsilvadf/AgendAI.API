using System.Linq.Expressions;
using System.Reflection;
using AgendAI.Domain.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AgendAI.Infra.Persistence.Converters;

/// <summary>
/// Converte enums do domínio para <see cref="string"/> no banco (snake_case ou <see cref="EnumJsonValueAttribute"/>).
/// </summary>
/// <seealso cref="EnumPersistenceStrategy"/>
internal static class EnumSnakeCaseConverter
{
    /// <inheritdoc cref="EnumPersistenceStrategy"/>
    public static ValueConverter<TEnum, string> Create<TEnum>()
        where TEnum : struct, Enum
    {
        Expression<Func<TEnum, string>> toProvider = value => ConvertToString(value);
        Expression<Func<string, TEnum>> fromProvider = value => ConvertFromString<TEnum>(value);
        return new ValueConverter<TEnum, string>(toProvider, fromProvider);
    }

    private static string ConvertToString<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        var field = typeof(TEnum).GetField(value.ToString()!);
        if (field is null)
            return value.ToString()!.ToLowerInvariant();

        return field.GetCustomAttribute<EnumJsonValueAttribute>()?.Value
            ?? SnakeCaseLowerJsonNamingPolicy.Instance.ConvertName(field.Name);
    }

    private static TEnum ConvertFromString<TEnum>(string value)
        where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var jsonValue = field.GetCustomAttribute<EnumJsonValueAttribute>()?.Value
                ?? SnakeCaseLowerJsonNamingPolicy.Instance.ConvertName(field.Name);

            if (string.Equals(jsonValue, value, StringComparison.OrdinalIgnoreCase))
                return (TEnum)field.GetValue(null)!;
        }

        throw new InvalidOperationException($"Valor '{value}' inválido para enum {typeof(TEnum).Name}.");
    }
}
