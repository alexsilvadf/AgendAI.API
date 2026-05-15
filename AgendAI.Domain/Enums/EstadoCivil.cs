using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>EstadoCivil</c> do frontend (<c>paciente.model.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<EstadoCivil>))]
public enum EstadoCivil
{
    Solteiro,
    Casado,
    Divorciado,
    Viuvo,
    Outro
}
