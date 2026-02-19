using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
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
        if (IsMissing(name))
        {
            return DomainError.Validation(Name.RequiredMessage, "Name");
        }

        var categoryIdIsInvalid = categoryId is null || categoryId.Value <= 0;
        if (categoryIdIsInvalid)
        {
            return DomainError.Validation(CategoryId.InvalidMessage, "CategoryId");
        }

        return new Dish(name, categoryId!);
    }

    public Result<Name, DomainError> ChangeName(Name changedName)
    {
        if (IsMissing(changedName))
        {
            return DomainError.Validation(Name.RequiredMessage, "Name");
        }

        if (Name == changedName)
        {
            return Name; //no-op
        }

        Name = changedName;
        return Name;
    }

    public Result<CategoryId, DomainError> ChangeCategory(CategoryId changedCategoryId)
    {
        var categoryIdIsInvalid = changedCategoryId is null || changedCategoryId.Value <= 0;
        if (categoryIdIsInvalid)
        {
            return DomainError.Validation(CategoryId.InvalidMessage, "CategoryId");
        }

        if (CategoryId == changedCategoryId)
        {
            return CategoryId; // no-op
        }

        CategoryId = changedCategoryId!;
        return CategoryId;
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
        CategoryId = null!;
    }
}