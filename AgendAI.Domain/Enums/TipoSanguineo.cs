using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>TipoSanguineo</c> do frontend (<c>paciente.model.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<TipoSanguineo>))]
public enum TipoSanguineo
{
    [EnumJsonValue("A+")]
    APositivo,

    [EnumJsonValue("A-")]
    ANegativo,

    [EnumJsonValue("B+")]
    BPositivo,

    [EnumJsonValue("B-")]
    BNegativo,

    [EnumJsonValue("AB+")]
    AbPositivo,

    [EnumJsonValue("AB-")]
    AbNegativo,

    [EnumJsonValue("O+")]
    OPositivo,

    [EnumJsonValue("O-")]
    ONegativo,

    NaoInformado
}
