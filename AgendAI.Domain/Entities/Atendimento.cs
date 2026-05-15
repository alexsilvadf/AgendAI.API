using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Atendimento : Entity
{
    public Guid? AgendamentoId { get; set; }

    public Agendamento? Agendamento { get; set; }

    public Guid ProfissionalId { get; set; }

    public Usuario Profissional { get; set; } = null!;

    public Guid PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public Guid ProcedimentoId { get; set; }

    public Procedimento Procedimento { get; set; } = null!;

    public DateOnly Data { get; set; }

    public TimeOnly Hora { get; set; }

    public decimal Valor { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public string Dentes { get; set; } = string.Empty;

    public bool Retorno { get; set; }

    public bool Pago { get; set; }

    public FormaPagamento? FormaPagamento { get; set; }

    public int? Parcelas { get; set; }

    public DateTime CriadoEm { get; set; }

    public Lancamento? Lancamento { get; set; }
}
