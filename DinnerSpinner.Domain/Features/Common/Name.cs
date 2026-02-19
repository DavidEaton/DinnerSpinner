using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Errors;

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

    public static Result<Name, DomainError> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name.Trim()) || name.Trim().Length == 0)
        {
            return DomainError.Validation(RequiredMessage, "Name");
        }

        string trimmedName = name.Trim();
        if (trimmedName.Length < MinimumLength || trimmedName.Length > MaximumLength)
        {
            return DomainError.Validation(InvalidLengthMessage, "Name");
        }

        return new Name(trimmedName);
    }

    public override string ToString() => Value;

    // Entity Framework requires a parameterless constructor for entity materialization
    private Name() =>
        Value = null!;
}