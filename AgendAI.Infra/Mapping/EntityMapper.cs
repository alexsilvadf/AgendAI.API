using AgendAI.Application.DTOs.Agendamentos;
using AgendAI.Application.DTOs.Atendimentos;
using AgendAI.Application.DTOs.Financeiro;
using AgendAI.Application.DTOs.Pacientes;
using AgendAI.Application.DTOs.Procedimentos;
using AgendAI.Application.DTOs.Usuarios;
using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;

namespace AgendAI.Infra.Mapping;

internal static class EntityMapper
{
    public static UsuarioDto ToDto(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Usuario = usuario.Login,
        Role = usuario.Role.ToJsonValue(),
        Especialidade = usuario.Especialidade,
        Ativo = usuario.Ativo,
        CriadoEm = usuario.CriadoEm.ToString("yyyy-MM-dd")
    };

    public static ProcedimentoDto ToDto(Procedimento procedimento) => new()
    {
        Id = procedimento.Id,
        Nome = procedimento.Nome,
        Valor = procedimento.Valor,
        Status = procedimento.Status.ToJsonValue()
    };

    public static PacienteResumoDto ToResumo(Paciente paciente) => new()
    {
        Id = paciente.Id,
        Nome = paciente.Nome,
        Cpf = FormatarCpf(paciente.Cpf),
        Telefone = paciente.Telefone,
        Ativo = paciente.Ativo
    };

    public static PacienteDto ToDto(Paciente paciente) => new()
    {
        Id = paciente.Id,
        Nome = paciente.Nome,
        Cpf = FormatarCpf(paciente.Cpf),
        DataNascimento = paciente.DataNascimento.ToString("yyyy-MM-dd"),
        Sexo = paciente.Sexo.ToJsonValue(),
        EstadoCivil = paciente.EstadoCivil.ToJsonValue(),
        Telefone = paciente.Telefone,
        Email = paciente.Email,
        Cep = paciente.Cep,
        Logradouro = paciente.Logradouro,
        Numero = paciente.Numero,
        Complemento = paciente.Complemento,
        Bairro = paciente.Bairro,
        Cidade = paciente.Cidade,
        Uf = paciente.Uf,
        TipoSanguineo = paciente.TipoSanguineo.ToJsonValue(),
        Anamnese = paciente.Anamnese is null ? null : new AnamneseDto
        {
            TemDoencaCardiaca = paciente.Anamnese.TemDoencaCardiaca,
            TemDiabetes = paciente.Anamnese.TemDiabetes,
            TemHipertensao = paciente.Anamnese.TemHipertensao,
            TemCoagulopatia = paciente.Anamnese.TemCoagulopatia,
            TemAlergiaMedicamento = paciente.Anamnese.TemAlergiaMedicamento,
            AlergiaMedicamentoDesc = paciente.Anamnese.AlergiaMedicamentoDesc,
            TemAlergiaMaterial = paciente.Anamnese.TemAlergiaMaterial,
            AlergiaMaterialDesc = paciente.Anamnese.AlergiaMaterialDesc,
            UsaMedicamentoContinuo = paciente.Anamnese.UsaMedicamentoContinuo,
            MedicamentoContinuoDesc = paciente.Anamnese.MedicamentoContinuoDesc,
            EstaGravida = paciente.Anamnese.EstaGravida,
            Fumante = paciente.Anamnese.Fumante,
            ObservacoesGerais = paciente.Anamnese.ObservacoesGerais
        },
        Historico = paciente.Historicos.Select(h => new HistoricoItemDto
        {
            Id = h.Id,
            Data = h.Data.ToString("yyyy-MM-dd"),
            Procedimento = h.Procedimento,
            Profissional = h.Profissional,
            Observacoes = h.Observacoes,
            Valor = h.Valor
        }).ToList(),
        Ativo = paciente.Ativo,
        CriadoEm = paciente.CriadoEm.ToString("yyyy-MM-dd"),
        AtualizadoEm = paciente.AtualizadoEm.ToString("yyyy-MM-dd")
    };

    public static AgendamentoDto ToDto(Agendamento agendamento) => new()
    {
        Id = agendamento.Id,
        ProfissionalId = agendamento.ProfissionalId,
        PacienteId = agendamento.PacienteId,
        ProcedimentoId = agendamento.ProcedimentoId,
        Data = agendamento.Data.ToString("yyyy-MM-dd"),
        HoraInicio = agendamento.HoraInicio.ToString("HH:mm"),
        HoraFim = agendamento.HoraFim.ToString("HH:mm"),
        Valor = agendamento.Valor,
        Status = agendamento.Status.ToJsonValue(),
        Observacoes = agendamento.Observacoes
    };

    public static AtendimentoDto ToDto(Atendimento atendimento) => new()
    {
        Id = atendimento.Id,
        ProfessionalId = atendimento.ProfissionalId,
        Profissional = atendimento.Profissional?.Nome ?? string.Empty,
        Paciente = atendimento.Paciente?.Nome ?? string.Empty,
        Procedimento = atendimento.Procedimento?.Nome ?? string.Empty,
        Data = atendimento.Data.ToString("yyyy-MM-dd"),
        Hora = atendimento.Hora.ToString("HH:mm"),
        Valor = atendimento.Valor,
        Observacoes = atendimento.Observacoes,
        Dentes = atendimento.Dentes,
        Retorno = atendimento.Retorno,
        Pago = atendimento.Pago,
        FormaPagamento = atendimento.FormaPagamento?.ToJsonValue(),
        Parcelas = atendimento.Parcelas,
        AgendamentoId = atendimento.AgendamentoId
    };

    public static LancamentoDto ToDto(Lancamento lancamento) => new()
    {
        Id = lancamento.Id,
        Tipo = lancamento.Tipo.ToJsonValue(),
        Descricao = lancamento.Descricao,
        Valor = lancamento.Valor,
        Data = lancamento.Data.ToString("yyyy-MM-dd"),
        Vencimento = lancamento.Vencimento.ToString("yyyy-MM-dd"),
        Status = lancamento.Status.ToJsonValue(),
        Categoria = lancamento.Categoria.ToJsonValue(),
        FormaPagamento = lancamento.FormaPagamento?.ToJsonValue(),
        AtendimentoId = lancamento.AtendimentoId,
        Paciente = lancamento.Paciente,
        Profissional = lancamento.Profissional,
        Procedimento = lancamento.Procedimento,
        Observacoes = lancamento.Observacoes,
        CriadoEm = lancamento.CriadoEm.ToString("yyyy-MM-dd")
    };

    public static string FormatarCpf(string cpf)
    {
        if (cpf.Length != 11)
            return cpf;

        return $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..]}";
    }
}
