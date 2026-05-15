using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>SlotStatus</c> do frontend (<c>agenda.types.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<SlotStatus>))]
public enum SlotStatus
{
    Livre,
    Ocupado,
    Indisponivel
}
