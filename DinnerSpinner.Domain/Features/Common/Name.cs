using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Domain.Features.Common;

public sealed record Name : IValueObject
{
    public const int MinimumLength = 2;
    public const int MaximumLength = 255;

    public string Value { get; }
    private Name(string value) => Value = value;

    public static Result<Name, Error> Create(string name) =>
            name is null
                ? Errors.ValidNameRequired()
                : name.Trim().Length.IsWithin(MinimumLength, MaximumLength)
                    ? new Name(name.Trim())
                    : Errors.ValidLengthName(MinimumLength, MaximumLength);

    public override string ToString() => Value;

    // Entity Framework requires a parameterless constructor for entity materialization
    private Name() => Value = null!;
}