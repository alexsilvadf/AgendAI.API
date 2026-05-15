using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
{
    private const string StatusAgendadoFilter = "[Status] = N'agendado'";

    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("Agendamentos");

        builder.HasKey(agendamento => agendamento.Id);

        builder.Property(agendamento => agendamento.Status)
            .HasConversion(EnumSnakeCaseConverter.Create<StatusAgendamento>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(agendamento => agendamento.Observacoes)
            .HasMaxLength(2000);

        builder.HasOne(agendamento => agendamento.Profissional)
            .WithMany(usuario => usuario.AgendamentosComoProfissional)
            .HasForeignKey(agendamento => agendamento.ProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(agendamento => agendamento.Paciente)
            .WithMany(paciente => paciente.Agendamentos)
            .HasForeignKey(agendamento => agendamento.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(agendamento => agendamento.Procedimento)
            .WithMany(procedimento => procedimento.Agendamentos)
            .HasForeignKey(agendamento => agendamento.ProcedimentoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Conflito: mesmo profissional não pode ter dois agendamentos ativos no mesmo horário.
        builder.HasIndex(agendamento => new { agendamento.ProfissionalId, agendamento.Data, agendamento.HoraInicio })
            .IsUnique()
            .HasDatabaseName("IX_Agendamentos_Profissional_Data_Hora_Agendado")
            .HasFilter(StatusAgendadoFilter);

        // Conflito: mesmo paciente não pode ter dois agendamentos ativos no mesmo horário.
        builder.HasIndex(agendamento => new { agendamento.PacienteId, agendamento.Data, agendamento.HoraInicio })
            .IsUnique()
            .HasDatabaseName("IX_Agendamentos_Paciente_Data_Hora_Agendado")
            .HasFilter(StatusAgendadoFilter);
    }
}
