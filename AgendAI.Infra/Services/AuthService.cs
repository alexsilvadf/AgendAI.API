using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Auth;
using AgendAI.Application.Security;
using AgendAI.Domain.Enums;
using AgendAI.Domain.Exceptions;
using AgendAI.Infra.Persistence;
using AgendAI.Infra.Security;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class AuthService(AgendAiDbContext db, JwtTokenGenerator tokenGenerator) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var login = request.Usuario.Trim().ToLowerInvariant();

        var usuario = await db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);

        if (usuario is null || !usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        var (token, expiresIn) = tokenGenerator.Generate(usuario);

        return new LoginResponse
        {
            Token = token,
            ExpiresIn = expiresIn,
            Nome = usuario.Nome,
            Usuario = usuario.Login,
            Role = usuario.Role.ToJsonValue(),
            Permissions = RolePermissions.GetPermissionNames(usuario.Role),
            ProfessionalId = usuario.Role == UserRole.Dentista ? usuario.Id : null
        };
    }
}
