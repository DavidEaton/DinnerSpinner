using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace DinnerSpinner.Api.Common;

internal static class SendExtensions
{
    public static Task OkAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        T data,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var traceId = GetTraceId(send.HttpContext);
        return send.ResponseAsync(ApiResponse<T>
            .Ok(data, traceId), StatusCodes.Status200OK, cancellationToken);
    }

    public static Task CreatedAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        T data,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var traceId = GetTraceId(send.HttpContext);
        return send.ResponseAsync(ApiResponse<T>
            .Ok(data, traceId), StatusCodes.Status201Created, cancellationToken);
    }

    // ----- ProblemDetails errors -----

    public static Task ValidationAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ProblemAsync(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Validation failed.",
            detail: message,
            type: "https://httpstatuses.com/400",
            cancellationToken);

    public static Task NotFoundAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ProblemAsync(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found.",
            detail: message,
            type: "https://httpstatuses.com/404",
            cancellationToken);

    public static Task ConflictAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ProblemAsync(
            statusCode: StatusCodes.Status409Conflict,
            title: "Conflict.",
            detail: message,
            type: "https://httpstatuses.com/409",
            cancellationToken);

    public static Task ForbiddenAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ProblemAsync(
            statusCode: StatusCodes.Status403Forbidden,
            title: "Forbidden.",
            detail: message,
            type: "https://httpstatuses.com/403",
            cancellationToken);

    public static Task UnexpectedAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ProblemAsync(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Unexpected error.",
            detail: message,
            type: "https://httpstatuses.com/500",
            cancellationToken);

    // Core helper: emits ProblemDetails + sets application/problem+json when possible.
    private static Task ProblemAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        int statusCode,
        string title,
        string detail,
        string type,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var http = send.HttpContext;
        var traceId = GetTraceId(http);

        var problem = new ProblemDetails(failures: [],
                                         instance: http?.Request?.Path.Value ?? string.Empty,
                                         traceId: traceId ?? string.Empty,
                                         statusCode: statusCode);

        // Ensure correct content-type if we can.
        http?.Response.ContentType = "application/problem+json";

        return send.HttpContext.Response.SendAsync(problem, statusCode, cancellation: cancellationToken);
    }

    private static string? GetTraceId(HttpContext? httpContext)
        => httpContext?.TraceIdentifier ?? Activity.Current?.Id;
}
