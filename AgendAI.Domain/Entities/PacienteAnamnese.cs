namespace AgendAI.Domain.Entities;

public class PacienteAnamnese
{
    public Guid PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

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
