using AgendAI.Application.DTOs.Procedimentos;

namespace AgendAI.Application.Abstractions;

public interface IProcedimentoService
{
    Task<IReadOnlyList<ProcedimentoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProcedimentoDto>> ListarAsync(CancellationToken cancellationToken = default);
}
