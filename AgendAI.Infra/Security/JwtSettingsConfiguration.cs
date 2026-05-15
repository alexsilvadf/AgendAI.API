using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AgendAI.Infra.Security;

internal static class JwtSettingsConfiguration
{
    /// <summary>
    /// Apenas para modo in-memory (testes/mock no Render). Nunca use em produção com SQL Server.
    /// </summary>
    internal const string InMemoryFallbackSecret =
        "INSECURE-AgendAI-in-memory-render-only-not-for-real-data";

    internal static void ApplyInMemoryFallbackIfNeeded(IConfiguration configuration, JwtSettings settings)
    {
        if (!DataOptions.UseInMemory(configuration))
            return;

        if (!string.IsNullOrWhiteSpace(settings.Secret) && settings.Secret.Length >= 32)
            return;

        settings.Secret = InMemoryFallbackSecret;
    }

    internal static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .PostConfigure(settings => ApplyInMemoryFallbackIfNeeded(configuration, settings))
            .PostConfigure<ILoggerFactory>((settings, loggerFactory) =>
            {
                if (!DataOptions.UseInMemory(configuration))
                    return;

                if (!string.Equals(settings.Secret, InMemoryFallbackSecret, StringComparison.Ordinal))
                    return;

                loggerFactory
                    .CreateLogger(typeof(JwtSettings))
                    .LogWarning(
                        "Jwt:Secret não configurado; usando chave insegura de fallback para modo in-memory. " +
                        "Defina Jwt__Secret (mín. 32 caracteres) para ambientes reais.");
            })
            .Validate(
                jwt => !string.IsNullOrWhiteSpace(jwt.Secret) && jwt.Secret.Length >= 32,
                "Jwt:Secret deve ter no mínimo 32 caracteres (configure Jwt__Secret no ambiente).")
            .Validate(
                jwt => !string.IsNullOrWhiteSpace(jwt.Issuer) && !string.IsNullOrWhiteSpace(jwt.Audience),
                "Jwt:Issuer e Jwt:Audience são obrigatórios.")
            .ValidateOnStart();
    }
}
