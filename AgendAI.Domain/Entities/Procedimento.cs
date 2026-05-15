using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Procedimento : Entity
{
    public string Nome { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public StatusProcedimento Status { get; set; } = StatusProcedimento.Ativo;

    public DateTime CriadoEm { get; set; }

    public ICollection<Agendamento> Agendamentos { get; set; } = [];

    public ICollection<Atendimento> Atendimentos { get; set; } = [];
}
