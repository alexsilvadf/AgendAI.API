using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class ProcedimentoConfiguration : IEntityTypeConfiguration<Procedimento>
{
    public void Configure(EntityTypeBuilder<Procedimento> builder)
    {
        builder.ToTable("Procedimentos");

        builder.HasKey(procedimento => procedimento.Id);

        builder.Property(procedimento => procedimento.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(procedimento => procedimento.Status)
            .HasConversion(EnumSnakeCaseConverter.Create<StatusProcedimento>())
            .HasMaxLength(50)
            .IsRequired();
    }
}
