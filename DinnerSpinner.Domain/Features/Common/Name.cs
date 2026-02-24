using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Domain.Features.Common;

public sealed record Name : IValueObject
{
    public const int MinimumLength = 2;
    public const int MaximumLength = 255;

    public static readonly string InvalidLengthMessage =
        $"Name must be between {MinimumLength} and {MaximumLength} characters in length";
    public static readonly string RequiredMessage = "Name is required";

    public string Value { get; }
    private Name(string value) => Value = value;

    public static Result<Name, Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.ValidNameRequired();

        var trimmed = name.Trim();

        if (trimmed.Length is < MinimumLength or > MaximumLength)
            return Errors.ValidNameRequired();

        return new Name(trimmed);
    }

    public override string ToString() => Value;

    // Entity Framework requires a parameterless constructor for entity materialization
    private Name() => Value = null!;
}