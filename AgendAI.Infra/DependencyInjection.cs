using System.Text;
using AgendAI.Application.Abstractions;
using AgendAI.Infra.Persistence;
using AgendAI.Infra.Security;
using AgendAI.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AgendAI.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (DataOptions.UseInMemory(configuration))
        {
            services.AddDbContext<AgendAiDbContext>(options =>
                options.UseInMemoryDatabase(DataOptions.InMemoryDatabaseName));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

            services.AddDbContext<AgendAiDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(
                jwt => !string.IsNullOrWhiteSpace(jwt.Secret) && jwt.Secret.Length >= 32,
                "Jwt:Secret deve ter no mínimo 32 caracteres (configure Jwt__Secret no ambiente).")
            .Validate(
                jwt => !string.IsNullOrWhiteSpace(jwt.Issuer) && !string.IsNullOrWhiteSpace(jwt.Audience),
                "Jwt:Issuer e Jwt:Audience são obrigatórios.")
            .ValidateOnStart();

        services.AddSingleton<JwtTokenGenerator>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAgendaService, AgendaService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IProcedimentoService, ProcedimentoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAtendimentoService, AtendimentoService>();
        services.AddScoped<IFinanceiroService, FinanceiroService>();

        var jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Configuração Jwt não encontrada.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
