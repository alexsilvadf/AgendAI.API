using AgendAI.Application.DTOs.Agendamentos;

namespace AgendAI.Application.Abstractions;

public interface IAgendamentoService
{
    Task<AgendamentoDto> CriarAsync(CriarAgendamentoRequest request, CancellationToken cancellationToken = default);

    Task<AgendamentoDto> CancelarAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AgendamentoDto> RemarcarAsync(Guid id, RemarcarAgendamentoRequest request, CancellationToken cancellationToken = default);
}
