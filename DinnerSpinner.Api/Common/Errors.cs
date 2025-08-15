namespace DinnerSpinner.Api.Common
{
    public class Errors
    {
        public enum ErrorCode
        {
            Validation,
            NotFound,
            Conflict,
            Forbidden,
            Unexpected
        }

        public record Error(ErrorCode Code, string Message)
        {
            public static Error Validation(string msg) => new(ErrorCode.Validation, msg);
            public static Error NotFound(string msg) => new(ErrorCode.NotFound, msg);
            public static Error Conflict(string msg) => new(ErrorCode.Conflict, msg);
            public static Error Forbidden(string msg) => new(ErrorCode.Forbidden, msg);
            public static Error Unexpected(string msg) => new(ErrorCode.Unexpected, msg);
        }
    }
}
