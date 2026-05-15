using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class BloqueioAgendaConfiguration : IEntityTypeConfiguration<BloqueioAgenda>
{
    public void Configure(EntityTypeBuilder<BloqueioAgenda> builder)
    {
        builder.ToTable("BloqueiosAgenda");

        builder.HasKey(bloqueio => bloqueio.Id);

        builder.Property(bloqueio => bloqueio.Motivo).HasMaxLength(500).IsRequired();

        builder.Property(bloqueio => bloqueio.Tipo)
            .HasConversion(EnumSnakeCaseConverter.Create<TipoBloqueioAgenda>())
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(bloqueio => bloqueio.Profissional)
            .WithMany(usuario => usuario.BloqueiosAgenda)
            .HasForeignKey(bloqueio => bloqueio.ProfissionalId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
