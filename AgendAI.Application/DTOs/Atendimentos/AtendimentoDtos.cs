namespace AgendAI.Application.DTOs.Atendimentos;

public sealed class AtendimentoDto
{
    public Guid Id { get; set; }

    public Guid ProfessionalId { get; set; }

    public string Profissional { get; set; } = string.Empty;

    public string Paciente { get; set; } = string.Empty;

    public string Procedimento { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public string Dentes { get; set; } = string.Empty;

    public bool Retorno { get; set; }

    public bool Pago { get; set; }

    public string? FormaPagamento { get; set; }

    public int? Parcelas { get; set; }

    public Guid? AgendamentoId { get; set; }
}

public sealed class CriarAtendimentoRequest
{
    public Guid ProfissionalId { get; set; }

    public Guid PacienteId { get; set; }

    public Guid ProcedimentoId { get; set; }

    public string Data { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public string Dentes { get; set; } = string.Empty;

    public bool Retorno { get; set; }

    public Guid? AgendamentoId { get; set; }
}

public sealed class RegistrarPagamentoRequest
{
    public string FormaPagamento { get; set; } = string.Empty;

    public int? Parcelas { get; set; }
}
