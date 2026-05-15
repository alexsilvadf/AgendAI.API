using AgendAI.Domain.Enums;

namespace AgendAI.Domain.Entities;

public class Paciente : Entity
{
    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public DateOnly DataNascimento { get; set; }

    public Sexo Sexo { get; set; }

    public EstadoCivil EstadoCivil { get; set; }

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cep { get; set; } = string.Empty;

    public string Logradouro { get; set; } = string.Empty;

    public string Numero { get; set; } = string.Empty;

    public string Complemento { get; set; } = string.Empty;

    public string Bairro { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public TipoSanguineo TipoSanguineo { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    public DateTime AtualizadoEm { get; set; }

    public PacienteAnamnese? Anamnese { get; set; }

    public ICollection<PacienteHistorico> Historicos { get; set; } = [];

    public ICollection<Agendamento> Agendamentos { get; set; } = [];

    public ICollection<Atendimento> Atendimentos { get; set; } = [];
}
