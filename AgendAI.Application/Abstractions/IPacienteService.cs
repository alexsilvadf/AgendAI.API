using AgendAI.Application.DTOs.Pacientes;

namespace AgendAI.Application.Abstractions;

public interface IPacienteService
{
    Task<IReadOnlyList<PacienteResumoDto>> ListarAsync(string? nome, CancellationToken cancellationToken = default);

    Task<PacienteDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
