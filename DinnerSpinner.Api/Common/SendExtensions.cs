using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;

namespace DinnerSpinner.Api.Common;

internal static class SendExtensions
{
    public static Task OkAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        T data,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ResponseAsync(ApiResponse<T>.Ok(data), 200, cancellationToken);

    public static Task CreatedAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        T data,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ResponseAsync(ApiResponse<T>.Ok(data), 201, cancellationToken);

    public static Task ErrorAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        ApiError error,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ResponseAsync(ApiResponse<T>.Fail(error), ToStatusCode(error.Code), cancellationToken);

    public static Task ValidationAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ErrorAsync(ApiError.Validation(message), cancellationToken);

    public static Task NotFoundAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ErrorAsync(ApiError.NotFound(message), cancellationToken);

    public static Task ConflictAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ErrorAsync(ApiError.Conflict(message), cancellationToken);

    public static Task ForbiddenAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ErrorAsync(ApiError.Forbidden(message), cancellationToken);

    public static Task UnexpectedAsync<TRequest, T>(
        this ResponseSender<TRequest, ApiResponse<T>> send,
        string message,
        CancellationToken cancellationToken)
        where TRequest : notnull
        => send.ErrorAsync(ApiError.Unexpected(message), cancellationToken);

    private static int ToStatusCode(ErrorCode code) => code switch
    {
        ErrorCode.Validation => 400,
        ErrorCode.NotFound   => 404,
        ErrorCode.Conflict   => 409,
        ErrorCode.Forbidden  => 403,
        _                    => 500
    };
}
