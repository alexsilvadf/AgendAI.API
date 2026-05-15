using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Pacientes;
using AgendAI.Domain.Exceptions;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class PacienteService(AgendAiDbContext db) : IPacienteService
{
    public async Task<IReadOnlyList<PacienteResumoDto>> ListarAsync(
        string? nome,
        CancellationToken cancellationToken = default)
    {
        var query = db.Pacientes.AsNoTracking().Where(p => p.Ativo);

        if (!string.IsNullOrWhiteSpace(nome))
        {
            var termo = nome.Trim().ToLowerInvariant();
            query = query.Where(p => p.Nome.ToLower().Contains(termo));
        }

        var pacientes = await query.OrderBy(p => p.Nome).ToListAsync(cancellationToken);
        return pacientes.Select(EntityMapper.ToResumo).ToList();
    }

    public async Task<PacienteDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var paciente = await db.Pacientes
            .AsNoTracking()
            .Include(p => p.Anamnese)
            .Include(p => p.Historicos)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return paciente is null ? null : EntityMapper.ToDto(paciente);
    }
}
