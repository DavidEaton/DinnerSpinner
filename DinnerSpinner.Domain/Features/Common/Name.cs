using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Abstractions;

namespace DinnerSpinner.Domain.Features.Common;

public sealed record Name : IValueObject
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
        name = (name ?? string.Empty).Trim();

        if (name.Length == 0)
        {
            return Result.Failure<Name>(RequiredMessage);
        }
        else
        {
            return name.Length is < MinimumLength or > MaximumLength
            ? Result.Failure<Name>(InvalidLengthMessage)
            : Result.Success(new Name(name));
        }
    }

    public override string ToString() => Value;

    // Entity Framework requires a parameterless constructor for entity materialization
    private Name() =>
        Value = null!;
}