using AgendAI.Domain.Serialization;
using AgendAI.Infra.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AgendAI.Infra.Persistence;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Aplica precisão decimal (18,2) em todas as propriedades monetárias <c>Valor</c>.
    /// </summary>
    public static void ApplyValorColumnPrecision(this ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(entityType => entityType.GetProperties())
            .Where(property => property.Name == "Valor" && property.ClrType == typeof(decimal)))
        {
            property.SetPrecision(DatabaseConstants.MoneyPrecision);
            property.SetScale(DatabaseConstants.MoneyScale);
        }
    }

    /// <summary>
    /// Valida que propriedades enum declaradas nas entidades usam conversão para string.
    /// </summary>
    public static void ValidateEnumStringStorage(this ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(entityType => entityType.GetProperties())
            .Where(property => property.ClrType.IsEnum))
        {
            var columnType = property.GetColumnType();
            var providerType = property.GetValueConverter()?.ProviderClrType;

            if (providerType != typeof(string) && columnType is not "nvarchar" and not "varchar")
            {
                throw new InvalidOperationException(
                    $"A propriedade '{property.DeclaringType.DisplayName()}.{property.Name}' deve usar " +
                    $"{nameof(EnumSnakeCaseConverter)} (armazenamento {EnumPersistenceStrategy.StorageType}, " +
                    $"formato {EnumPersistenceStrategy.ValueFormat}). Ver {nameof(EnumPersistenceStrategy)}.");
            }
        }
    }

    /// <summary>
    /// Garante Restrict em todas as FKs (evita cascata acidental em entidades principais).
    /// </summary>
    public static void ApplyRestrictOnDelete(this ModelBuilder modelBuilder)
    {
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(entityType => entityType.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
