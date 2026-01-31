using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish : Entity
{
    public Category Category { get; private set; } = null!;
    public Name Name { get; private set; } = null!;


    private Dish(Name name, Category category)
    {
        Name = name;
        Category = category;
    }

    public static Result<Dish> Create(Name name, Category category)
    {
        {
            if (name is null)
                return Result.Failure<Dish>("Name is required.");

            if (category is null)
                return Result.Failure<Dish>("Category is required.");

            return Result.Success(new Dish(name, category));
        }
    }

    public Result Rename(Name newName)
    {
        if (newName is null)
            return Result.Failure("Name is required.");

        Name = newName;
        return Result.Success();
    }

    public Result ChangeCategory(Category newCategory)
    {
        if (newCategory is null)
            return Result.Failure("Category is required.");

        Category = newCategory;
        return Result.Success();
    }

    // Entity Framework requires an empty constructor
    protected Dish() { }
}