using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AgendAI.Domain.Serialization;

public sealed class SnakeCaseEnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly Dictionary<TEnum, string> EnumToJson = BuildEnumToJson();
    private static readonly Dictionary<string, TEnum> JsonToEnum = BuildJsonToEnum();

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Expected string for enum {typeof(TEnum).Name}.");

        var value = reader.GetString();

        if (value is not null && JsonToEnum.TryGetValue(value, out var enumValue))
            return enumValue;

        throw new JsonException($"Unknown value '{value}' for enum {typeof(TEnum).Name}.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (!EnumToJson.TryGetValue(value, out var jsonValue))
            throw new JsonException($"Unknown enum value '{value}' for {typeof(TEnum).Name}.");

        writer.WriteStringValue(jsonValue);
    }

    private static Dictionary<TEnum, string> BuildEnumToJson()
    {
        var map = new Dictionary<TEnum, string>();

        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var enumValue = (TEnum)field.GetValue(null)!;
            var jsonValue = field.GetCustomAttribute<EnumJsonValueAttribute>()?.Value
                ?? SnakeCaseLowerJsonNamingPolicy.Instance.ConvertName(field.Name);

            map[enumValue] = jsonValue;
        }

        return map;
    }

    private static Dictionary<string, TEnum> BuildJsonToEnum()
    {
        return BuildEnumToJson()
            .GroupBy(pair => pair.Value, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First().Key, StringComparer.OrdinalIgnoreCase);
    }
}
