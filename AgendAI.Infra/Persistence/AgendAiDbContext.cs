using AgendAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Persistence;

public class AgendAiDbContext(DbContextOptions<AgendAiDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Paciente> Pacientes => Set<Paciente>();

    public DbSet<PacienteAnamnese> PacienteAnamneses => Set<PacienteAnamnese>();

    public DbSet<PacienteHistorico> PacienteHistoricos => Set<PacienteHistorico>();

    public DbSet<Procedimento> Procedimentos => Set<Procedimento>();

    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    public DbSet<BloqueioAgenda> BloqueiosAgenda => Set<BloqueioAgenda>();

    public DbSet<Atendimento> Atendimentos => Set<Atendimento>();

    public DbSet<Lancamento> Lancamentos => Set<Lancamento>();

    public DbSet<TokenRecuperacaoSenha> TokensRecuperacaoSenha => Set<TokenRecuperacaoSenha>();

    public DbSet<ConfiguracaoClinica> ConfiguracoesClinica => Set<ConfiguracaoClinica>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendAiDbContext).Assembly);
        modelBuilder.ApplyValorColumnPrecision();
        modelBuilder.ValidateEnumStringStorage();
        modelBuilder.ApplyRestrictOnDelete();
        base.OnModelCreating(modelBuilder);
    }
}
