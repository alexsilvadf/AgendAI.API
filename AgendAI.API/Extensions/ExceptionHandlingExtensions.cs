using AgendAI.API.Middleware;

namespace AgendAI.API.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddAgendAIExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }

    public static IApplicationBuilder UseAgendAIExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
