namespace AgendAI.Infra.Persistence;

/// <summary>
/// Documenta a estratégia de persistência de enums no AgendAI.
/// </summary>
/// <remarks>
/// <para><b>Escolha: STRING (snake_case), não INT.</b></para>
/// <list type="bullet">
///   <item>Banco: colunas <c>nvarchar</c> (ex.: <c>administrador</c>, <c>agendado</c>, <c>cartao_credito_parcelado</c>).</item>
///   <item>API JSON: mesmos literais do frontend Angular (<c>user.model.ts</c>, <c>financeiro.service.ts</c>, etc.).</item>
///   <item>EF Core: <see cref="Converters.EnumSnakeCaseConverter{TEnum}"/> em cada propriedade enum nas Fluent configurations.</item>
///   <item>Exceções com literal fixo: <c>[EnumJsonValue]</c> no Domain (ex.: <c>agenda:view</c>, <c>A+</c>).</item>
/// </list>
/// <para><b>Por que não INT?</b> O frontend envia/recebe strings; índices filtrados e seeds SQL
/// usam literais legíveis (<c>Status = 'agendado'</c>); reordenar membros do enum não corrompe dados históricos.</para>
/// <para><b>Conversor único:</b> regras alinhadas a <see cref="AgendAI.Domain.Serialization.SnakeCaseLowerJsonNamingPolicy"/>
/// e <see cref="AgendAI.Domain.Serialization.EnumJsonValueAttribute"/>.</para>
/// </remarks>
public static class EnumPersistenceStrategy
{
    /// <summary>Tipo lógico armazenado no banco para todos os enums de domínio.</summary>
    public const string StorageType = "string";

    /// <summary>Formato padrão dos valores (ex.: <c>nao_informado</c>, <c>cartao_debito</c>).</summary>
    public const string ValueFormat = "snake_case_lower";

    /// <summary>Tamanho padrão das colunas enum nas migrations (ajustar por enum se necessário).</summary>
    public const int DefaultMaxLength = 50;
}
