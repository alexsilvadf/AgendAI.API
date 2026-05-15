namespace AgendAI.Application.DTOs.Usuarios;

public sealed class UsuarioDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? Especialidade { get; set; }

    public bool Ativo { get; set; }

    public string CriadoEm { get; set; } = string.Empty;
}
