namespace AgendAI.Domain.Entities;

public class ConfiguracaoClinica
{
    public int Id { get; set; } = 1;

    public TimeOnly HoraAbertura { get; set; }

    public TimeOnly HoraFechamento { get; set; }

    public int IntervaloMinutos { get; set; } = 30;
}
