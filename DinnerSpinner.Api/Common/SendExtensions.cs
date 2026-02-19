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

    private static string? GetTraceId(HttpContext? httpContext)
        => httpContext?.TraceIdentifier ?? Activity.Current?.Id;
}
