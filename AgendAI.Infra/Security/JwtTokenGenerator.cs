using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendAI.Application.Security;
using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgendAI.Infra.Security;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> options)
{
    private readonly JwtSettings _settings = options.Value;

    public (string Token, int ExpiresInSeconds) Generate(Usuario usuario)
    {
        var permissions = RolePermissions.GetPermissionNames(usuario.Role);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, usuario.Login),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Role, usuario.Role.ToJsonValue())
        };

        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        if (usuario.Role == UserRole.Dentista)
            claims.Add(new Claim("professionalId", usuario.Id.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), (int)_settings.ExpirationMinutes * 60);
    }
}
