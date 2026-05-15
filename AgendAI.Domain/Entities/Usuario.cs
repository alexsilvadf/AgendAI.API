using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Usuario : Entity
{
    public string Nome { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public string? Especialidade { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    public ICollection<Agendamento> AgendamentosComoProfissional { get; set; } = [];

    public ICollection<Atendimento> AtendimentosComoProfissional { get; set; } = [];

    public ICollection<BloqueioAgenda> BloqueiosAgenda { get; set; } = [];

    public ICollection<TokenRecuperacaoSenha> TokensRecuperacaoSenha { get; set; } = [];
}
