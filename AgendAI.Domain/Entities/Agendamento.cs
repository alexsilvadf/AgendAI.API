using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Agendamento : Entity
{
    public Guid ProfissionalId { get; set; }

    public Usuario Profissional { get; set; } = null!;

    public Guid PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public Guid ProcedimentoId { get; set; }

    public Procedimento Procedimento { get; set; } = null!;

    public DateOnly Data { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFim { get; set; }

    public decimal Valor { get; set; }

    public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;

    public string? Observacoes { get; set; }

    public DateTime CriadoEm { get; set; }

    public DateTime AtualizadoEm { get; set; }

    public Atendimento? Atendimento { get; set; }
}
