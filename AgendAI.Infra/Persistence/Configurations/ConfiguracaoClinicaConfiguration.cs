using AgendAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class ConfiguracaoClinicaConfiguration : IEntityTypeConfiguration<ConfiguracaoClinica>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoClinica> builder)
    {
        builder.ToTable("ConfiguracoesClinica");

        builder.HasKey(configuracao => configuracao.Id);

        builder.HasData(new ConfiguracaoClinica
        {
            Id = 1,
            HoraAbertura = new TimeOnly(8, 0),
            HoraFechamento = new TimeOnly(18, 0),
            IntervaloMinutos = 30
        });
    }
}
