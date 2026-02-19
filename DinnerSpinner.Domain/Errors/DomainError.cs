namespace DinnerSpinner.Domain.Errors;

/// <summary>
/// Domain-level error. Not HTTP-aware. Structured enough to map to API/ProblemDetails.
/// </summary>
public readonly record struct DomainError(
    ErrorCode Code,
    string Message,
    string? Field = null,
    IReadOnlyDictionary<string, object?>? Meta = null)
{
    public static DomainError Validation(string message, string? field = null)
        => new(ErrorCode.Validation, message, field);

    public static DomainError NotFound(string message, string? field = null)
        => new(ErrorCode.NotFound, message, field);

    public static DomainError Conflict(string message, string? field = null)
        => new(ErrorCode.Conflict, message, field);

    public static DomainError Forbidden(string message, string? field = null)
        => new(ErrorCode.Forbidden, message, field);

    public static DomainError Unexpected(string message, string? field = null)
        => new(ErrorCode.Unexpected, message, field);

    public override string ToString()
        => Field is null ? $"{Code}: {Message}" : $"{Code}({Field}): {Message}";

    public static implicit operator ErrorCode(DomainError error) => error.Code;
    public override int GetHashCode() => HashCode.Combine(Code, Message);
}