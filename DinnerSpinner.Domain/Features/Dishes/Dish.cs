using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish : Entity
{
    public Name Name { get; private set; }
    public Category Category { get; private set; }

    private Dish(Name name, Category category)
    {
        Name = name;
        Category = category;
    }

    public static Result<Dish, Error> Create(Name name, Category category) =>
        name is null
            ? Errors.ValidNameRequired()
            : (category is null)
                ? Errors.ValidCategoryRequired()
                 : new Dish(name, category);

    public Result<Dish, Error> ChangeName(Name changedName) =>
        changedName is null
            ? Errors.ValidNameRequired()
            : (Name == changedName)
                ? this // no-op
                : Mutate(changedName);
                
    private Result<Dish, Error> Mutate(Name changedName)
    {
        Name = changedName;
        return this;
    }

    public Result<Dish, Error> ChangeCategory(Category changedCategory) =>
        changedCategory is null
            ? Errors.ValidCategoryRequired()
            : (Category == changedCategory)
                ? this // no-op
                : Mutate(changedCategory);

    private Result<Dish, Error> Mutate(Category changedCategory)
    {
        Category = changedCategory;
        return this;
    }

    public override string ToString() => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Dish()
    {
        //Non-nullable property 'Name' must contain a non-null value when exiting constructor.
        Name = null!;
        // Non-nullable property 'Category' must contain a non-null value when exiting constructor.
        Category = null!;
    }
}