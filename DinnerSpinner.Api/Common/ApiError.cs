namespace DinnerSpinner.Api.Common
{
    public sealed record ApiError(ErrorCode Code, string Message)
    {
        public static ApiError Validation(string msg) => new(ErrorCode.Validation, msg);
        public static ApiError NotFound(string msg) => new(ErrorCode.NotFound, msg);
        public static ApiError Conflict(string msg) => new(ErrorCode.Conflict, msg);
        public static ApiError Forbidden(string msg) => new(ErrorCode.Forbidden, msg);
        public static ApiError Unexpected(string msg) => new(ErrorCode.Unexpected, msg);
    }
}