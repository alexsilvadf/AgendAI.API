using AgendAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class PacienteAnamneseConfiguration : IEntityTypeConfiguration<PacienteAnamnese>
{
    public void Configure(EntityTypeBuilder<PacienteAnamnese> builder)
    {
        builder.ToTable("PacienteAnamneses");

        builder.HasKey(anamnese => anamnese.PacienteId);

        builder.Property(anamnese => anamnese.AlergiaMedicamentoDesc).HasMaxLength(500);
        builder.Property(anamnese => anamnese.AlergiaMaterialDesc).HasMaxLength(500);
        builder.Property(anamnese => anamnese.MedicamentoContinuoDesc).HasMaxLength(500);
        builder.Property(anamnese => anamnese.ObservacoesGerais).HasMaxLength(2000);
    }
}
