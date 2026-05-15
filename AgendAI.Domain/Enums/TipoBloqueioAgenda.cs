using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Tipo de bloqueio de agenda (Almoco|Admin no modelo de dados).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<TipoBloqueioAgenda>))]
public enum TipoBloqueioAgenda
{
    Almoco,
    Admin
}
