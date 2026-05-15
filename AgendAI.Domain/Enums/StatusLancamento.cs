using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>StatusLancamento</c> do frontend (<c>financeiro.service.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<StatusLancamento>))]
public enum StatusLancamento
{
    Pendente,
    Pago,
    Cancelado
}
