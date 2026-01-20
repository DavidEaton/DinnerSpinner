namespace DinnerSpinner.Api.Common;

public sealed record ApiResponse<T>(
    T? Data,
    ApiError? Error)
{
    public static ApiResponse<T> Ok(T data) => new(data, null);
    public static ApiResponse<T> Fail(ApiError error) => new(default, error);
}
