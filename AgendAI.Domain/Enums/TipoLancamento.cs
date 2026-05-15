using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>TipoLancamento</c> do frontend (<c>financeiro.service.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<TipoLancamento>))]
public enum TipoLancamento
{
    Receita,
    Despesa
}
