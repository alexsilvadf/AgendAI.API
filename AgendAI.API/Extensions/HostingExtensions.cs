using Microsoft.AspNetCore.HttpOverrides;

namespace AgendAI.API.Extensions;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureCloudHosting(this WebApplicationBuilder builder)
    {
        var port = Environment.GetEnvironmentVariable("PORT");
        if (!string.IsNullOrWhiteSpace(port))
        {
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
        }

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
        });

        return builder;
    }

    public static WebApplication UseCloudHosting(this WebApplication app)
    {
        app.UseForwardedHeaders();

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PORT")))
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
}
