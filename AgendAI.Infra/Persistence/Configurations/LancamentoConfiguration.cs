using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendAI.Infra.Persistence.Configurations;

public class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
{
    public void Configure(EntityTypeBuilder<Lancamento> builder)
    {
        builder.ToTable("Lancamentos");

        builder.HasKey(lancamento => lancamento.Id);

        builder.Property(lancamento => lancamento.Descricao).HasMaxLength(300).IsRequired();
        builder.Property(lancamento => lancamento.Paciente).HasMaxLength(200);
        builder.Property(lancamento => lancamento.Profissional).HasMaxLength(200);
        builder.Property(lancamento => lancamento.Procedimento).HasMaxLength(200);
        builder.Property(lancamento => lancamento.Observacoes).HasMaxLength(2000);

        builder.Property(lancamento => lancamento.Tipo)
            .HasConversion(EnumSnakeCaseConverter.Create<TipoLancamento>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(lancamento => lancamento.Status)
            .HasConversion(EnumSnakeCaseConverter.Create<StatusLancamento>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(lancamento => lancamento.Categoria)
            .HasConversion(EnumSnakeCaseConverter.Create<CategoriaLancamento>())
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(lancamento => lancamento.FormaPagamento)
            .HasConversion(EnumSnakeCaseConverter.Create<FormaPagamento>())
            .HasMaxLength(50);

        builder.HasOne(lancamento => lancamento.Atendimento)
            .WithOne(atendimento => atendimento.Lancamento)
            .HasForeignKey<Lancamento>(lancamento => lancamento.AtendimentoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(lancamento => lancamento.AtendimentoId)
            .IsUnique()
            .HasDatabaseName("IX_Lancamentos_AtendimentoId")
            .HasFilter("[AtendimentoId] IS NOT NULL");
    }
}
