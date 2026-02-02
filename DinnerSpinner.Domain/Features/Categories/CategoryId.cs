using CSharpFunctionalExtensions;

namespace DinnerSpinner.Domain.Features.Categories;

public sealed record CategoryId
{
    private const string DisplayName = nameof(CategoryId);

    public int Value { get; }

    private CategoryId(int value)
        => Value = value;

    public static Result<CategoryId> Create(int value)
    {
        if (value <= 0)
        {
            return Result.Failure<CategoryId>($"{DisplayName} must be a positive integer.");
        }

        return Result.Success(new CategoryId(value));
    }

    public override string ToString() => Value.ToString();

    // Entity Framework requires an empty constructor
    private CategoryId() { }
}