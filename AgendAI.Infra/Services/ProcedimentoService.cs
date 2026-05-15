using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Procedimentos;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class ProcedimentoService(AgendAiDbContext db) : IProcedimentoService
{
    public async Task<IReadOnlyList<ProcedimentoDto>> ListarAtivosAsync(
        CancellationToken cancellationToken = default) =>
        await ListarInternoAsync(ativo: true, cancellationToken);

    public async Task<IReadOnlyList<ProcedimentoDto>> ListarAsync(
        CancellationToken cancellationToken = default) =>
        await ListarInternoAsync(ativo: null, cancellationToken);

    private async Task<IReadOnlyList<ProcedimentoDto>> ListarInternoAsync(
        bool? ativo,
        CancellationToken cancellationToken)
    {
        var query = db.Procedimentos.AsNoTracking().AsQueryable();

        if (ativo == true)
            query = query.Where(p => p.Status == StatusProcedimento.Ativo);

        var itens = await query.OrderBy(p => p.Nome).ToListAsync(cancellationToken);
        return itens.Select(EntityMapper.ToDto).ToList();
    }
}
