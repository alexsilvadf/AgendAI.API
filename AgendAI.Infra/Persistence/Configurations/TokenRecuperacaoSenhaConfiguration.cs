using AgendAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class TokenRecuperacaoSenhaConfiguration : IEntityTypeConfiguration<TokenRecuperacaoSenha>
{
    public void Configure(EntityTypeBuilder<TokenRecuperacaoSenha> builder)
    {
        builder.ToTable("TokensRecuperacaoSenha");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.TokenHash).HasMaxLength(500).IsRequired();

        builder.HasOne(token => token.Usuario)
            .WithMany(usuario => usuario.TokensRecuperacaoSenha)
            .HasForeignKey(token => token.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
