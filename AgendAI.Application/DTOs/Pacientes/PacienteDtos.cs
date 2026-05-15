namespace AgendAI.Application.DTOs.Pacientes;

public sealed class AnamneseDto
{
    public bool TemDoencaCardiaca { get; set; }

    public bool TemDiabetes { get; set; }

    public bool TemHipertensao { get; set; }

    public bool TemCoagulopatia { get; set; }

    public bool TemAlergiaMedicamento { get; set; }

    public string AlergiaMedicamentoDesc { get; set; } = string.Empty;

    public bool TemAlergiaMaterial { get; set; }

    public string AlergiaMaterialDesc { get; set; } = string.Empty;

    public bool UsaMedicamentoContinuo { get; set; }

    public string MedicamentoContinuoDesc { get; set; } = string.Empty;

    public bool EstaGravida { get; set; }

    public bool Fumante { get; set; }

    public string ObservacoesGerais { get; set; } = string.Empty;
}

public sealed class HistoricoItemDto
{
    public Guid Id { get; set; }

    public string Data { get; set; } = string.Empty;

    public string Procedimento { get; set; } = string.Empty;

    public string Profissional { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public decimal Valor { get; set; }
}

public sealed class PacienteDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string DataNascimento { get; set; } = string.Empty;

    public string Sexo { get; set; } = string.Empty;

    public string EstadoCivil { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cep { get; set; } = string.Empty;

    public string Logradouro { get; set; } = string.Empty;

    public string Numero { get; set; } = string.Empty;

    public string Complemento { get; set; } = string.Empty;

    public string Bairro { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public string TipoSanguineo { get; set; } = string.Empty;

    public AnamneseDto? Anamnese { get; set; }

    public IReadOnlyList<HistoricoItemDto> Historico { get; set; } = [];

    public bool Ativo { get; set; }

    public string CriadoEm { get; set; } = string.Empty;

    public string AtualizadoEm { get; set; } = string.Empty;
}

public sealed class PacienteResumoDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public bool Ativo { get; set; }
}
