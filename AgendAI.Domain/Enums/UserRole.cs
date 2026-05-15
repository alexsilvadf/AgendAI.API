using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>UserRole</c> do frontend (<c>user.model.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<UserRole>))]
public enum UserRole
{
    Administrador,
    Dentista,
    Recepcionista
}
