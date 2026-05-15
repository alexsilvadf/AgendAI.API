using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Financeiro;
using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Domain.Exceptions;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class FinanceiroService(AgendAiDbContext db) : IFinanceiroService
{
    public async Task<IReadOnlyList<LancamentoDto>> ListarAsync(
        DateOnly? dataInicio,
        DateOnly? dataFim,
        string? tipo,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = db.Lancamentos.AsNoTracking().AsQueryable();

        if (dataInicio.HasValue)
            query = query.Where(l => l.Data >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(l => l.Data <= dataFim.Value);

        if (!string.IsNullOrWhiteSpace(tipo))
        {
            var tipoEnum = EnumExtensions.FromJsonValue<TipoLancamento>(tipo);
            query = query.Where(l => l.Tipo == tipoEnum);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var statusEnum = EnumExtensions.FromJsonValue<StatusLancamento>(status);
            query = query.Where(l => l.Status == statusEnum);
        }

        var itens = await query
            .OrderByDescending(l => l.Data)
            .ThenByDescending(l => l.CriadoEm)
            .ToListAsync(cancellationToken);

        return itens.Select(EntityMapper.ToDto).ToList();
    }

    public async Task<LancamentoDto> CriarAsync(
        CriarLancamentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var tipo = EnumExtensions.FromJsonValue<TipoLancamento>(request.Tipo);
        var status = EnumExtensions.FromJsonValue<StatusLancamento>(request.Status);
        var categoria = EnumExtensions.FromJsonValue<CategoriaLancamento>(request.Categoria);

        FormaPagamento? forma = null;
        if (!string.IsNullOrWhiteSpace(request.FormaPagamento))
            forma = EnumExtensions.FromJsonValue<FormaPagamento>(request.FormaPagamento);

        var lancamento = new Lancamento
        {
            Id = Guid.NewGuid(),
            Tipo = tipo,
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Data = DateOnly.Parse(request.Data),
            Vencimento = DateOnly.Parse(request.Vencimento),
            Status = status,
            Categoria = categoria,
            FormaPagamento = forma,
            Observacoes = request.Observacoes,
            CriadoEm = DateTime.UtcNow
        };

        db.Lancamentos.Add(lancamento);
        await db.SaveChangesAsync(cancellationToken);

        return EntityMapper.ToDto(lancamento);
    }

    public async Task<LancamentoDto> AtualizarStatusAsync(
        Guid id,
        AtualizarStatusLancamentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var lancamento = await db.Lancamentos
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken)
            ?? throw new NotFoundException("Lançamento", id);

        lancamento.Status = EnumExtensions.FromJsonValue<StatusLancamento>(request.Status);
        await db.SaveChangesAsync(cancellationToken);

        return EntityMapper.ToDto(lancamento);
    }
}
