using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Lancamento : Entity
{
    public TipoLancamento Tipo { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public DateOnly Data { get; set; }

    public DateOnly Vencimento { get; set; }

    public StatusLancamento Status { get; set; }

    public CategoriaLancamento Categoria { get; set; }

    public FormaPagamento? FormaPagamento { get; set; }

    public Guid? AtendimentoId { get; set; }

    public Atendimento? Atendimento { get; set; }

    public string? Paciente { get; set; }

    public string? Profissional { get; set; }

    public string? Procedimento { get; set; }

    public string? Observacoes { get; set; }

    public DateTime CriadoEm { get; set; }
}
