using AgendAI.Application.DTOs.Financeiro;

namespace AgendAI.Application.Abstractions;

public interface IFinanceiroService
{
    Task<IReadOnlyList<LancamentoDto>> ListarAsync(
        DateOnly? dataInicio,
        DateOnly? dataFim,
        string? tipo,
        string? status,
        CancellationToken cancellationToken = default);

    Task<LancamentoDto> CriarAsync(
        CriarLancamentoRequest request,
        CancellationToken cancellationToken = default);

    Task<LancamentoDto> AtualizarStatusAsync(
        Guid id,
        AtualizarStatusLancamentoRequest request,
        CancellationToken cancellationToken = default);
}
