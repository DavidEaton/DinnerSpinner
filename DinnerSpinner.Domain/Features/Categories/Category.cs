using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; }
    private Category(Name name) => Name = name;

    public static Result<Category, Error> Create(Name name) =>
        name is null
            ? Errors.ValidCategoryRequired()
            : new Category(name);

    public Result<Category, Error> ChangeName(Name changedName) =>
        changedName is null
            ? Errors.ValidCategoryRequired()
            : (Name == changedName)
                ? this // no-op
                : Mutate(changedName);

    private Result<Category, Error> Mutate(Name changedName)
    {
        Name = changedName;
        return this;
    }

    public override string ToString() => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Category() => Name = null!;
}
