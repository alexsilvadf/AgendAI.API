using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>EtapaAtendimento</c> do frontend (<c>atendimento.component.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<EtapaAtendimento>))]
public enum EtapaAtendimento
{
    Agenda,
    Ficha,
    Procedimento,
    Finalizado
}
