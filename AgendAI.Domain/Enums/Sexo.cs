using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>Sexo</c> do frontend (<c>paciente.model.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<Sexo>))]
public enum Sexo
{
    Masculino,
    Feminino,
    Outro,
    NaoInformado
}
