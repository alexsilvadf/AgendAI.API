using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");

        builder.HasKey(paciente => paciente.Id);

        builder.Property(paciente => paciente.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(paciente => paciente.Cpf)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(paciente => paciente.Telefone).HasMaxLength(20);
        builder.Property(paciente => paciente.Email).HasMaxLength(200);
        builder.Property(paciente => paciente.Cep).HasMaxLength(8);
        builder.Property(paciente => paciente.Logradouro).HasMaxLength(200);
        builder.Property(paciente => paciente.Numero).HasMaxLength(20);
        builder.Property(paciente => paciente.Complemento).HasMaxLength(100);
        builder.Property(paciente => paciente.Bairro).HasMaxLength(100);
        builder.Property(paciente => paciente.Cidade).HasMaxLength(100);
        builder.Property(paciente => paciente.Uf).HasMaxLength(2);

        builder.Property(paciente => paciente.Sexo)
            .HasConversion(EnumSnakeCaseConverter.Create<Sexo>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(paciente => paciente.EstadoCivil)
            .HasConversion(EnumSnakeCaseConverter.Create<EstadoCivil>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(paciente => paciente.TipoSanguineo)
            .HasConversion(EnumSnakeCaseConverter.Create<TipoSanguineo>())
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(paciente => paciente.Cpf)
            .IsUnique()
            .HasDatabaseName("IX_Pacientes_Cpf");

        builder.HasOne(paciente => paciente.Anamnese)
            .WithOne(anamnese => anamnese.Paciente)
            .HasForeignKey<PacienteAnamnese>(anamnese => anamnese.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
