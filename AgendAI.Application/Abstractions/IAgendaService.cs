using AgendAI.Application.DTOs.Agenda;
using AgendAI.Domain.Enums;

namespace AgendAI.Application.Abstractions;

public interface IAgendaService
{
    Task<IReadOnlyList<ProfessionalDto>> ListarProfissionaisAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DayScheduleDto>> MontarGradeAsync(
        DateOnly data,
        Guid? profissionalId,
        UserRole role,
        Guid? usuarioLogadoId,
        CancellationToken cancellationToken = default);
}
