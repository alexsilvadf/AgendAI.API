using System.Text;
using System.Text.Json;

namespace AgendAI.Domain.Serialization;

/// <summary>
/// Converte nomes PascalCase para snake_case minúsculo (ex.: CartaoCreditoParcelado → cartao_credito_parcelado).
/// </summary>
public sealed class SnakeCaseLowerJsonNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseLowerJsonNamingPolicy Instance { get; } = new();

    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var buffer = new StringBuilder(name.Length + 8);

        for (var i = 0; i < name.Length; i++)
        {
            var character = name[i];

            if (char.IsUpper(character))
            {
                if (i > 0)
                    buffer.Append('_');

                buffer.Append(char.ToLowerInvariant(character));
            }
            else
            {
                buffer.Append(character);
            }
        }

        return buffer.ToString();
    }
}
