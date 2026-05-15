using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Status persistido de um agendamento (espelha regras do backend e da agenda).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<StatusAgendamento>))]
public enum StatusAgendamento
{
    Agendado,
    Cancelado,
    Remarcado,
    Atendido
}
