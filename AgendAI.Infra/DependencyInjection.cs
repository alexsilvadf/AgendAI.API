using AgendAI.Application.Abstractions;
using AgendAI.Infra.Persistence;
using AgendAI.Infra.Security;
using AgendAI.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        JwtSettingsConfiguration.Register(services, configuration);

        services.AddSingleton<JwtTokenGenerator>();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAgendaService, AgendaService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IProcedimentoService, ProcedimentoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAtendimentoService, AtendimentoService>();
        services.AddScoped<IFinanceiroService, FinanceiroService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();

        return services;
    }
}
