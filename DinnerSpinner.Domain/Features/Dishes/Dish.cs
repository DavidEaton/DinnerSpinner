using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

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
        static Result<Dish> Fail(string message) =>
            Result.Failure<Dish>(message);

        if (name is null || string.IsNullOrWhiteSpace(name.Value))
            return Fail(Name.RequiredMessage);

        if (categoryId.Value <= 0)
            return Fail(CategoryId.InvalidMessage);

        return Result.Success(new Dish(name, categoryId));
    }

    public Result ChangeName(Name newName)
    {
        if (Name == newName)
        {
            return Result.Success(); //no-op
        }

        Name = newName;
        return Result.Success();
    }

    public Result ChangeCategory(CategoryId newCategoryId)
    {
        if (CategoryId == newCategoryId)
        {
            return Result.Success("Category updated successfully."); // no-op
        }

        CategoryId = newCategoryId; // State mutation. Side effects should be explicit. "Mutating aggregate state."
        return Result.Success(); // Result signaling. Returns status, not data.
    }

    public override string ToString()
        => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Dish()
    {
         //Non-nullable property 'Name' must contain a non-null value when exiting constructor.
        Name = null!;
        // Non-nullable property 'CategoryId' must contain a non-null value when exiting constructor.
        CategoryId = value;
    }
}