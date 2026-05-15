using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>CategoriaDespesa</c> do frontend (<c>financeiro.service.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<CategoriaDespesa>))]
public enum CategoriaDespesa
{
    Aluguel,
    Salario,
    Material,
    Equipamento,
    Servico,
    Imposto,
    Outros
}
