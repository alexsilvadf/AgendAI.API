using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Status do cadastro de procedimento (Ativo/Inativo no modelo de dados).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<StatusProcedimento>))]
public enum StatusProcedimento
{
    Ativo,
    Inativo
}
