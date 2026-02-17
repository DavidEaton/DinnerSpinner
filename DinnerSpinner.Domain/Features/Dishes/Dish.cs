using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish : Entity
{
    private const CategoryId value = null;

    public Name Name { get; private set; }
    public CategoryId CategoryId { get; private set; }

    private Dish(Name name, CategoryId categoryId)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public static Result<Dish> Create(Name name, CategoryId categoryId)
    {
        // local semantic alias for Result.Failure<Dish> to simplify guard clauses
        static Result<Dish> Fail(string message) =>
            Result.Failure<Dish>(message);

        if (IsMissing(name))
            return Fail(Name.RequiredMessage);

        if (categoryId.Value <= 0)
            return Fail(CategoryId.InvalidMessage);

        return Result.Success(new Dish(name, categoryId));
    }

    public Result<Name> ChangeName(Name changedName)
    {
        static Result<Name> Fail(string message)
        {
            return Result.Failure<Name>(message);
        }

        if (IsMissing(changedName))
        {
            return Fail(Name.RequiredMessage);
        }

        if (Name == changedName)
        {
            return Result.Success(Name); //no-op
        }

        Name = changedName;
        return Result.Success(Name);
    }

    public Result<CategoryId> ChangeCategory(CategoryId changedCategoryId)
    {
        if (CategoryId == changedCategoryId)
        {
            return Result.Success(CategoryId); // no-op
        }

        CategoryId = changedCategoryId; // State mutation. Side effects should be explicit. "Mutating aggregate state."
        return Result.Success(CategoryId); // Result signaling. Returns status, not data.
    }

    public override string ToString()
        => Name.ToString();

    private static bool IsMissing(Name name) =>
        name is null || string.IsNullOrWhiteSpace(name.Value);

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Dish()
    {
        //Non-nullable property 'Name' must contain a non-null value when exiting constructor.
        Name = null!;
        // Non-nullable property 'CategoryId' must contain a non-null value when exiting constructor.
        CategoryId = value;
    }
}