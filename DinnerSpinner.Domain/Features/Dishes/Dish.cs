using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish : Entity
{
    public Name Name { get; private set; }
    public CategoryId CategoryId { get; private set; }

    private Dish(Name name, CategoryId categoryId)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public static Result<Dish, Error> Create(Name name, CategoryId categoryId) =>
        name is null
            ? Errors.ValidNameRequired()
            : (categoryId is null)
                ? Errors.ValidCategoryRequired()
                 : new Dish(name, categoryId);

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

    public Result<Dish, Error> ChangeCategory(CategoryId changedCategoryId) =>
        changedCategoryId is null
            ? Errors.ValidCategoryRequired()
            : (CategoryId == changedCategoryId)
                ? this // no-op
                : Mutate(changedCategoryId);

    private Result<Dish, Error> Mutate(CategoryId changedCategoryId)
    {
        CategoryId = changedCategoryId;
        return this;
    }

    public override string ToString() => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Dish()
    {
        //Non-nullable property 'Name' must contain a non-null value when exiting constructor.
        Name = null!;
        // Non-nullable property 'CategoryId' must contain a non-null value when exiting constructor.
        CategoryId = null!;
    }
}