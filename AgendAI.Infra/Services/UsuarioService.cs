using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Usuarios;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class UsuarioService(AgendAiDbContext db) : IUsuarioService
{
    public async Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await db.Usuarios
            .AsNoTracking()
            .OrderBy(u => u.Nome)
            .ToListAsync(cancellationToken);

        return usuarios.Select(EntityMapper.ToDto).ToList();
    }
}
