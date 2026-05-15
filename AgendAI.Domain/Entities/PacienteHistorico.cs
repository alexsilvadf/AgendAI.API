namespace AgendAI.Domain.Entities;

public class PacienteHistorico : Entity
{
    public Guid PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public DateOnly Data { get; set; }

    public string Procedimento { get; set; } = string.Empty;

    public string Profissional { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public decimal Valor { get; set; }
}
