using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>Permission</c> do frontend (<c>user.model.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<Permission>))]
public enum Permission
{
    [EnumJsonValue("agenda:view")]
    AgendaView,

    [EnumJsonValue("agenda:edit")]
    AgendaEdit,

    [EnumJsonValue("agendamento:create")]
    AgendamentoCreate,

    [EnumJsonValue("agendamento:cancel")]
    AgendamentoCancel,

    [EnumJsonValue("pacientes:view")]
    PacientesView,

    [EnumJsonValue("pacientes:edit")]
    PacientesEdit,

    [EnumJsonValue("procedimentos:view")]
    ProcedimentosView,

    [EnumJsonValue("procedimentos:edit")]
    ProcedimentosEdit,

    [EnumJsonValue("financeiro:view")]
    FinanceiroView,

    [EnumJsonValue("financeiro:edit")]
    FinanceiroEdit,

    [EnumJsonValue("usuarios:view")]
    UsuariosView,

    [EnumJsonValue("usuarios:edit")]
    UsuariosEdit,

    [EnumJsonValue("atendimento:create")]
    AtendimentoCreate
}
