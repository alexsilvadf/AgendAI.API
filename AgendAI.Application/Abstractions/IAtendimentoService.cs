using AgendAI.Application.DTOs.Atendimentos;

namespace AgendAI.Application.Abstractions;

public interface IAtendimentoService
{
    Task<IReadOnlyList<AtendimentoDto>> ListarAsync(
        DateOnly? data,
        Guid? profissionalId,
        bool? pago,
        CancellationToken cancellationToken = default);

    Task<AtendimentoDto> CriarAsync(
        CriarAtendimentoRequest request,
        CancellationToken cancellationToken = default);

    Task<AtendimentoDto> RegistrarPagamentoAsync(
        Guid id,
        RegistrarPagamentoRequest request,
        CancellationToken cancellationToken = default);
}
