using CSharpFunctionalExtensions;

namespace DinnerSpinner.Domain.Features.Common;

public sealed record Name
{
    public const int MinimumLength = 2;
    public const int MaximumLength = 255;
    public static readonly string InvalidLengthMessage =
        $"Name must be between {MinimumLength} and {MaximumLength} characters in length";
    public static readonly string RequiredMessage = "Name is required";
    
    public string Value { get; private set; }
    private Name(string value) => Value = value;

    public static Result<Name> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>(RequiredMessage);

        var trimmed = name.Trim();

        if (trimmed.Length is < MinimumLength or > MaximumLength)
            return Result.Failure<Name>(InvalidLengthMessage);

        return Result.Success(new Name(trimmed));
    }

    public override string ToString() => Value;

    // Entity Framework requires a parameterless constructor for entity materialization
    private Name() =>
        Value = null!;
}