using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class AtendimentoConfiguration : IEntityTypeConfiguration<Atendimento>
{
    public void Configure(EntityTypeBuilder<Atendimento> builder)
    {
        builder.ToTable("Atendimentos");

        builder.HasKey(atendimento => atendimento.Id);

        builder.Property(atendimento => atendimento.Observacoes).HasMaxLength(2000);
        builder.Property(atendimento => atendimento.Dentes).HasMaxLength(200);

        builder.Property(atendimento => atendimento.FormaPagamento)
            .HasConversion(EnumSnakeCaseConverter.Create<FormaPagamento>())
            .HasMaxLength(50);

        builder.HasOne(atendimento => atendimento.Agendamento)
            .WithOne(agendamento => agendamento.Atendimento)
            .HasForeignKey<Atendimento>(atendimento => atendimento.AgendamentoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(atendimento => atendimento.AgendamentoId)
            .IsUnique()
            .HasDatabaseName("IX_Atendimentos_AgendamentoId")
            .HasFilter("[AgendamentoId] IS NOT NULL");

        builder.HasOne(atendimento => atendimento.Profissional)
            .WithMany(usuario => usuario.AtendimentosComoProfissional)
            .HasForeignKey(atendimento => atendimento.ProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(atendimento => atendimento.Paciente)
            .WithMany(paciente => paciente.Atendimentos)
            .HasForeignKey(atendimento => atendimento.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(atendimento => atendimento.Procedimento)
            .WithMany(procedimento => procedimento.Atendimentos)
            .HasForeignKey(atendimento => atendimento.ProcedimentoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(atendimento => new { atendimento.ProfissionalId, atendimento.Data, atendimento.Hora })
            .IsUnique()
            .HasDatabaseName("IX_Atendimentos_Profissional_Data_Hora");
    }
}
