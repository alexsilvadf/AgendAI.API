namespace AgendAI.Application.DTOs.Agendamentos;

public sealed class CriarAgendamentoRequest
{
    public Guid ProfissionalId { get; set; }

    public Guid PacienteId { get; set; }

    public Guid ProcedimentoId { get; set; }

    public string Data { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public decimal? Valor { get; set; }

    public string? Observacoes { get; set; }
}

public sealed class RemarcarAgendamentoRequest
{
    public string NovaHora { get; set; } = string.Empty;
}

public sealed class AgendamentoDto
{
    public Guid Id { get; set; }

    public Guid ProfissionalId { get; set; }

    public Guid PacienteId { get; set; }

    public Guid ProcedimentoId { get; set; }

    public string Data { get; set; } = string.Empty;

    public string HoraInicio { get; set; } = string.Empty;

    public string HoraFim { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Observacoes { get; set; }
}
