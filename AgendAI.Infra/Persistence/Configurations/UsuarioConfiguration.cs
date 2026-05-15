using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(usuario => usuario.Login)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(usuario => usuario.SenhaHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(usuario => usuario.Role)
            .HasConversion(EnumSnakeCaseConverter.Create<UserRole>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(usuario => usuario.Especialidade)
            .HasMaxLength(200);

        builder.HasIndex(usuario => usuario.Login)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Login");
    }
}
