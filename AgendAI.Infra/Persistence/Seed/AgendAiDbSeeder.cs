using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Persistence.Seed;

public static class AgendAiDbSeeder
{
    public static async Task SeedAsync(AgendAiDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Usuarios.AnyAsync(cancellationToken))
            return;

        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(DateTime.Today);

        var usuarios = CriarUsuarios(now);
        var pacientes = CriarPacientes(now);
        var procedimentos = CriarProcedimentos(now);

        db.Usuarios.AddRange(usuarios);
        db.Pacientes.AddRange(pacientes);
        db.Procedimentos.AddRange(procedimentos);
        await db.SaveChangesAsync(cancellationToken);

        db.BloqueiosAgenda.AddRange(CriarBloqueiosAlmoco(today, usuarios));
        db.Agendamentos.AddRange(CriarAgendamentos(today, now, pacientes, procedimentos));
        await db.SaveChangesAsync(cancellationToken);

        var atendimentoPendente = CriarAtendimentoPendente(today, now);
        db.Atendimentos.Add(atendimentoPendente);
        db.Lancamentos.AddRange(CriarLancamentos(now, atendimentoPendente));
        await db.SaveChangesAsync(cancellationToken);
    }

    private static List<Usuario> CriarUsuarios(DateTime now) =>
    [
        new()
        {
            Id = SeedIds.UsuarioAdmin,
            Nome = "Admin Sistema",
            Login = "admin",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = UserRole.Administrador,
            Ativo = true,
            CriadoEm = now
        },
        new()
        {
            Id = SeedIds.UsuarioAnaMartins,
            Nome = "Dra. Ana Martins",
            Login = "ana.martins",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
            Role = UserRole.Dentista,
            Especialidade = "Clínica geral",
            Ativo = true,
            CriadoEm = now
        },
        new()
        {
            Id = SeedIds.UsuarioBrunoCosta,
            Nome = "Dr. Bruno Costa",
            Login = "bruno.costa",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
            Role = UserRole.Dentista,
            Especialidade = "Cardiologia",
            Ativo = true,
            CriadoEm = now
        },
        new()
        {
            Id = SeedIds.UsuarioCarlaRecepcao,
            Nome = "Carla Recepção",
            Login = "carla",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
            Role = UserRole.Recepcionista,
            Ativo = true,
            CriadoEm = now
        },
        new()
        {
            Id = SeedIds.UsuarioCarlaDias,
            Nome = "Dra. Carla Dias",
            Login = "carla.dias",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
            Role = UserRole.Dentista,
            Especialidade = "Dermatologia",
            Ativo = false,
            CriadoEm = now
        }
    ];

    private static List<Paciente> CriarPacientes(DateTime now)
    {
        var pacienteMaria = new Paciente
        {
            Id = SeedIds.PacienteMaria,
            Nome = "Maria Silva Santos",
            Cpf = "12345678900",
            DataNascimento = new DateOnly(1985, 3, 15),
            Sexo = Sexo.Feminino,
            EstadoCivil = EstadoCivil.Casado,
            Telefone = "(11) 98765-4321",
            Email = "maria.silva@email.com",
            Cep = "01310100",
            Logradouro = "Av. Paulista",
            Numero = "1000",
            Complemento = "Apto 42",
            Bairro = "Bela Vista",
            Cidade = "São Paulo",
            Uf = "SP",
            TipoSanguineo = TipoSanguineo.OPositivo,
            Ativo = true,
            CriadoEm = now,
            AtualizadoEm = now,
            Anamnese = new PacienteAnamnese
            {
                PacienteId = SeedIds.PacienteMaria,
                TemHipertensao = true,
                UsaMedicamentoContinuo = true,
                MedicamentoContinuoDesc = "Losartana 50mg"
            },
            Historicos =
            [
                new PacienteHistorico
                {
                    Id = Guid.NewGuid(),
                    PacienteId = SeedIds.PacienteMaria,
                    Data = new DateOnly(2025, 2, 10),
                    Procedimento = "Consulta clínica geral",
                    Profissional = "Dra. Ana Martins",
                    Observacoes = "Paciente relatou dor de cabeça frequente.",
                    Valor = 180
                }
            ]
        };

        var pacienteJoao = new Paciente
        {
            Id = SeedIds.PacienteJoao,
            Nome = "João Pedro Oliveira",
            Cpf = "98765432100",
            DataNascimento = new DateOnly(1990, 7, 22),
            Sexo = Sexo.Masculino,
            EstadoCivil = EstadoCivil.Solteiro,
            Telefone = "(11) 91234-5678",
            Email = "joao.pedro@email.com",
            Cep = "04038001",
            Logradouro = "Rua Vergueiro",
            Numero = "500",
            Bairro = "Vila Mariana",
            Cidade = "São Paulo",
            Uf = "SP",
            TipoSanguineo = TipoSanguineo.APositivo,
            Ativo = true,
            CriadoEm = now,
            AtualizadoEm = now,
            Anamnese = new PacienteAnamnese
            {
                PacienteId = SeedIds.PacienteJoao,
                Fumante = true
            }
        };

        var pacienteAna = new Paciente
        {
            Id = SeedIds.PacienteAnaLuiza,
            Nome = "Ana Luiza Ferreira",
            Cpf = "45678912300",
            DataNascimento = new DateOnly(1978, 11, 5),
            Sexo = Sexo.Feminino,
            EstadoCivil = EstadoCivil.Divorciado,
            Telefone = "(11) 97777-8888",
            Email = "ana.luiza@email.com",
            Cep = "05402100",
            Logradouro = "Rua Oscar Freire",
            Numero = "200",
            Complemento = "Casa",
            Bairro = "Jardins",
            Cidade = "São Paulo",
            Uf = "SP",
            TipoSanguineo = TipoSanguineo.BPositivo,
            Ativo = true,
            CriadoEm = now,
            AtualizadoEm = now,
            Anamnese = new PacienteAnamnese
            {
                PacienteId = SeedIds.PacienteAnaLuiza,
                TemDiabetes = true,
                TemAlergiaMedicamento = true,
                AlergiaMedicamentoDesc = "Dipirona"
            }
        };

        return [pacienteMaria, pacienteJoao, pacienteAna];
    }

    private static List<Procedimento> CriarProcedimentos(DateTime now) =>
    [
        new() { Id = SeedIds.ProcedimentoConsultaGeral, Nome = "Consulta clínica geral", Valor = 180, Status = StatusProcedimento.Ativo, CriadoEm = now },
        new() { Id = SeedIds.ProcedimentoRetorno, Nome = "Consulta de retorno", Valor = 120, Status = StatusProcedimento.Ativo, CriadoEm = now },
        new() { Id = SeedIds.ProcedimentoCardio, Nome = "Avaliação cardiológica", Valor = 350, Status = StatusProcedimento.Ativo, CriadoEm = now },
        new() { Id = SeedIds.ProcedimentoEletro, Nome = "Eletrocardiograma", Valor = 90, Status = StatusProcedimento.Ativo, CriadoEm = now },
        new() { Id = SeedIds.ProcedimentoDermato, Nome = "Consulta dermatológica", Valor = 220, Status = StatusProcedimento.Ativo, CriadoEm = now },
        new() { Id = SeedIds.ProcedimentoCirurgiaDermato, Nome = "Pequena cirurgia dermatológica", Valor = 450, Status = StatusProcedimento.Ativo, CriadoEm = now }
    ];

    private static IEnumerable<BloqueioAgenda> CriarBloqueiosAlmoco(DateOnly anchor, List<Usuario> usuarios)
    {
        var dentistas = usuarios.Where(u => u.Role == UserRole.Dentista && u.Ativo).ToList();
        var dias = new[] { anchor, anchor.AddDays(1), anchor.AddDays(2) };

        foreach (var dia in dias)
        foreach (var dentista in dentistas)
        {
            yield return new BloqueioAgenda
            {
                Id = Guid.NewGuid(),
                ProfissionalId = dentista.Id,
                Data = dia,
                HoraInicio = new TimeOnly(12, 0),
                HoraFim = new TimeOnly(13, 0),
                Motivo = "Almoço",
                Tipo = TipoBloqueioAgenda.Almoco
            };
        }
    }

    private static List<Agendamento> CriarAgendamentos(
        DateOnly anchor,
        DateTime now,
        List<Paciente> pacientes,
        List<Procedimento> procedimentos)
    {
        var consulta = procedimentos.First(p => p.Id == SeedIds.ProcedimentoConsultaGeral);
        var maria = pacientes.First(p => p.Id == SeedIds.PacienteMaria);
        var joao = pacientes.First(p => p.Id == SeedIds.PacienteJoao);

        return
        [
            CriarAgendamento(SeedIds.UsuarioAnaMartins, maria.Id, consulta, anchor, new TimeOnly(9, 0), now),
            CriarAgendamento(SeedIds.UsuarioAnaMartins, joao.Id, consulta, anchor, new TimeOnly(10, 0), now),
            CriarAgendamento(SeedIds.UsuarioBrunoCosta, maria.Id, procedimentos.First(p => p.Id == SeedIds.ProcedimentoCardio), anchor.AddDays(1), new TimeOnly(14, 0), now),
            CriarAgendamento(SeedIds.UsuarioBrunoCosta, pacientes.First(p => p.Id == SeedIds.PacienteAnaLuiza).Id, consulta, anchor.AddDays(2), new TimeOnly(11, 0), now)
        ];
    }

    private static Agendamento CriarAgendamento(
        Guid profissionalId,
        Guid pacienteId,
        Procedimento procedimento,
        DateOnly data,
        TimeOnly horaInicio,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            ProfissionalId = profissionalId,
            PacienteId = pacienteId,
            ProcedimentoId = procedimento.Id,
            Data = data,
            HoraInicio = horaInicio,
            HoraFim = horaInicio.AddMinutes(30),
            Valor = procedimento.Valor,
            Status = StatusAgendamento.Agendado,
            Observacoes = "Agendamento seed",
            CriadoEm = now,
            AtualizadoEm = now
        };

    private static Atendimento CriarAtendimentoPendente(DateOnly today, DateTime now)
    {
        var hora = new TimeOnly(15, 0);
        return new Atendimento
        {
            Id = SeedIds.AtendimentoPendenteAna,
            ProfissionalId = SeedIds.UsuarioAnaMartins,
            PacienteId = SeedIds.PacienteMaria,
            ProcedimentoId = SeedIds.ProcedimentoConsultaGeral,
            Data = today,
            Hora = hora,
            Valor = 180,
            Observacoes = "Atendimento concluído — aguardando pagamento",
            Dentes = "",
            Retorno = false,
            Pago = false,
            CriadoEm = now
        };
    }

    private static List<Lancamento> CriarLancamentos(DateTime now, Atendimento atendimentoPendente) =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoLancamento.Despesa,
            Descricao = "Aluguel consultório",
            Valor = 2500,
            Data = DateOnly.FromDateTime(DateTime.Today),
            Vencimento = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
            Status = StatusLancamento.Pendente,
            Categoria = CategoriaLancamento.Aluguel,
            CriadoEm = now
        },
        new()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoLancamento.Despesa,
            Descricao = "Material odontológico",
            Valor = 450,
            Data = DateOnly.FromDateTime(DateTime.Today),
            Vencimento = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
            Status = StatusLancamento.Pendente,
            Categoria = CategoriaLancamento.Material,
            CriadoEm = now
        },
        new()
        {
            Id = Guid.NewGuid(),
            Tipo = TipoLancamento.Receita,
            Descricao = "Receita manual — venda produto",
            Valor = 80,
            Data = DateOnly.FromDateTime(DateTime.Today),
            Vencimento = DateOnly.FromDateTime(DateTime.Today),
            Status = StatusLancamento.Pago,
            Categoria = CategoriaLancamento.Outros,
            FormaPagamento = FormaPagamento.Pix,
            CriadoEm = now
        }
    ];
}
