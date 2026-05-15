using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Categoria de lançamento financeiro (<c>CategoriaDespesa | 'atendimento'</c> no frontend).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<CategoriaLancamento>))]
public enum CategoriaLancamento
{
    Atendimento,
    Aluguel,
    Salario,
    Material,
    Equipamento,
    Servico,
    Imposto,
    Outros
}
