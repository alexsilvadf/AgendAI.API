namespace AgendAI.Application.DTOs.Financeiro;

public sealed class LancamentoDto
{
    public Guid Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Data { get; set; } = string.Empty;

    public string Vencimento { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string? FormaPagamento { get; set; }

    public Guid? AtendimentoId { get; set; }

    public string? Paciente { get; set; }

    public string? Profissional { get; set; }

    public string? Procedimento { get; set; }

    public string? Observacoes { get; set; }

    public string CriadoEm { get; set; } = string.Empty;
}

public sealed class CriarLancamentoRequest
{
    public string Tipo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Data { get; set; } = string.Empty;

    public string Vencimento { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string? FormaPagamento { get; set; }

    public string? Observacoes { get; set; }
}

public sealed class AtualizarStatusLancamentoRequest
{
    public string Status { get; set; } = string.Empty;
}
