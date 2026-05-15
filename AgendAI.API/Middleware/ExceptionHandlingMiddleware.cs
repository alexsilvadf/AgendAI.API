using System.Diagnostics;
using AgendAI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            LogException(exception, context);

            await WriteProblemDetailsAsync(context, exception);
        }
    }

    private void LogException(Exception exception, HttpContext context)
    {
        var message = "Exception on {Method} {Path}";

        if (exception is DomainException { StatusCode: < 500 })
        {
            logger.LogWarning(exception, message, context.Request.Method, context.Request.Path);
            return;
        }

        logger.LogError(exception, message, context.Request.Method, context.Request.Path);
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail, type) = MapException(exception);
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        if (exception is ValidationException validation)
        {
            var validationProblem = new ValidationProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = type,
                Instance = context.Request.Path,
                Errors = validation.Errors.ToDictionary(e => e.Key, e => e.Value)
            };

            EnrichProblemDetails(validationProblem, traceId, exception);
            await WriteResponseAsync(context, statusCode, validationProblem);
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = type,
            Instance = context.Request.Path
        };

        EnrichProblemDetails(problemDetails, traceId, exception);
        await WriteResponseAsync(context, statusCode, problemDetails);
    }

    private void EnrichProblemDetails(ProblemDetails problemDetails, string traceId, Exception exception)
    {
        problemDetails.Extensions["traceId"] = traceId;

        if (environment.IsDevelopment())
        {
            problemDetails.Extensions["exception"] = exception.GetType().FullName;
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        object problemDetails)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static (int StatusCode, string Title, string Detail, string? Type) MapException(Exception exception) =>
        exception switch
        {
            NotFoundException notFound => (
                notFound.StatusCode,
                notFound.Title,
                notFound.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
            ConflictException conflict => (
                conflict.StatusCode,
                conflict.Title,
                conflict.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
            ValidationException validation => (
                validation.StatusCode,
                validation.Title,
                validation.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            ForbiddenException forbidden => (
                forbidden.StatusCode,
                forbidden.Title,
                forbidden.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.3"),
            DomainException domain => (
                domain.StatusCode,
                domain.Title,
                domain.Message,
                GetDefaultType(domain.StatusCode)),
            ArgumentException argument => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                argument.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message,
                "https://tools.ietf.org/html/rfc7235#section-3.1"),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.",
                "https://tools.ietf.org/html/rfc7231#section-6.6.1")
        };

    private static string? GetDefaultType(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        StatusCodes.Status403Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
        StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        StatusCodes.Status500InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        _ => null
    };
}
