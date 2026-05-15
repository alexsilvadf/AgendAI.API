namespace AgendAI.API.Extensions;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder AddAgendAIConfiguration(this WebApplicationBuilder builder)
    {
        var environmentName = builder.Environment.EnvironmentName;

        builder.Configuration.Sources.Clear();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>(optional: true);
        }

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        return builder;
    }
}
