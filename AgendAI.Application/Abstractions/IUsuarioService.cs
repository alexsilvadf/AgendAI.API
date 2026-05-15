using AgendAI.Application.DTOs.Usuarios;

namespace AgendAI.Application.Abstractions;

public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken cancellationToken = default);
}
