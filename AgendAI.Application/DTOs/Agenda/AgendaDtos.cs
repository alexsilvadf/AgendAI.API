namespace AgendAI.Application.DTOs.Agenda;

public sealed class ProfessionalDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;
}

public sealed class AgendaSlotDto
{
    public string Start { get; set; } = string.Empty;

    public string End { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? PatientName { get; set; }

    public string? Detail { get; set; }

    public bool PendentePagamento { get; set; }

    public Guid? AgendamentoId { get; set; }

    public Guid? AtendimentoId { get; set; }
}

public sealed class DayScheduleDto
{
    public string Date { get; set; } = string.Empty;

    public Guid ProfessionalId { get; set; }

    public IReadOnlyList<AgendaSlotDto> Slots { get; set; } = [];
}
