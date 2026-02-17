namespace DinnerSpinner.Api.Common;

/// <summary>
/// Success envelope used only for 2xx responses.
/// Errors are returned as ProblemDetails (application/problem+json).
/// </summary>
public sealed record ApiResponse<T>(
    T Data,
    string? TraceId = null,
    object? Meta = null)
{
    public static ApiResponse<T> Ok(T data, string? traceId = null, object? meta = null)
        => new(data, traceId, meta);
}