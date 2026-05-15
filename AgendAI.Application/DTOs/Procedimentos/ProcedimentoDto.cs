namespace AgendAI.Application.DTOs.Procedimentos;

public sealed class ProcedimentoDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Status { get; set; } = string.Empty;
}
