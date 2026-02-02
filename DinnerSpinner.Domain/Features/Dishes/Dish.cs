using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish : Entity
{
    public Name Name { get; private set; } = null!;
    public CategoryId CategoryId { get; private set; } = null!;

    private Dish(Name name, CategoryId categoryId)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public static Result<Dish> Create(Name name, CategoryId categoryId)
        => Result.Success(new Dish(name, categoryId));

    public Result Rename(Name newName)
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
            return Result.Success(); //no-op
        }

        CategoryId = newCategoryId;
        return Result.Success();
    }

    // Entity Framework requires an empty constructor
    protected Dish() { }
}