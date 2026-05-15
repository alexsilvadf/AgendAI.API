namespace AgendAI.Application.DTOs.Auth;

public sealed class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public IReadOnlyList<string> Permissions { get; set; } = [];

    public Guid? ProfessionalId { get; set; }
}
