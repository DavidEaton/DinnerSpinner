using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.BaseClasses;

namespace DinnerSpinner.Domain.Features.Common;

public class Name : AppValueObject
{
    public const int MinimumLength = 2;
    public const int MaximumLength = 255;

    public static readonly string InvalidLengthMessage =
        $"Name must be between {MinimumLength} and {MaximumLength} characters in length";

    public static readonly string RequiredMessage = "Name is required";

    private Name(string value) => Value = value;

    public string Value { get; private set; } = null!;

    public static Result<Name> Create(string name) => CreateCore(name);

    // // If you still want this for semantic intent, keep it, but delegate.
    // public static Result<Name> NewName(string newName) => CreateCore(newName);

    private static Result<Name> CreateCore(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return Result.Failure<Name>(RequiredMessage);

        var trimmed = raw.Trim();

        if (trimmed.Length is < MinimumLength or > MaximumLength)
            return Result.Failure<Name>(InvalidLengthMessage);

        return Result.Success(new Name(trimmed));
    }

    public override string ToString() => Value;

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    // Entity Framework requires an empty constructor
    protected Name() { }
}