using CSharpFunctionalExtensions;

namespace DinnerSpinner.Domain.Features.Categories;

public sealed record CategoryId
{
    private const string DisplayName = nameof(CategoryId);
    internal const string InvalidMessage = $"{DisplayName} must be a positive integer.";

    public int Value { get; }

    private CategoryId(int value)
        => Value = value;

    public static Result<CategoryId> Create(int value)
    {
        if (value <= 0)
        {
            return Result.Failure<CategoryId>(InvalidMessage);
        }

        return Result.Success(new CategoryId(value));
    }

    public override string ToString() => Value.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    private CategoryId() { }
}