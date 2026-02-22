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

    public static Result<Dish, DomainError> Create(Name name, CategoryId categoryId)
    {
        if (name is null)
        {
            return DomainError.Validation(Name.RequiredMessage);
        }

        if (categoryId is null)
        {
            return DomainError.Validation(CategoryId.RequiredMessage);
        }

        return new Dish(name, categoryId);
    }

    public Result<Dish, DomainError> ChangeName(Name changedName)
    {
        if (changedName is null)
        {
            return DomainError.Validation(Name.RequiredMessage);
        }

        if (Name == changedName)
        {
            return this; //no-op
        }

        Name = changedName;
        return this;
    }

    public Result<CategoryId, DomainError> ChangeCategory(CategoryId changedCategoryId)
    {
        if (changedCategoryId is null)
        {
            return DomainError.Validation(CategoryId.RequiredMessage);
        }

        if (CategoryId == changedCategoryId)
        {
            return CategoryId; // no-op
        }

        CategoryId = changedCategoryId!;
        return CategoryId;
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