using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AgendAI.Domain.Enums;

namespace AgendAI.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? user.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.Parse(id!);
    }

    public static UserRole GetUserRole(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role)
            ?? throw new UnauthorizedAccessException("Role não encontrada no token.");

        return EnumExtensions.FromJsonValue<UserRole>(role);
    }

    public static bool HasPermission(this ClaimsPrincipal user, string permission) =>
        user.FindAll("permission").Any(c => c.Value == permission);
}
