using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; }
    private Category(Name name) => Name = name;

    public static Result<Category, DomainError> Create(Name name)
    {
        if (name is null)
        {
            return DomainError.Validation(Name.RequiredMessage);
        }

        return new Category(name);
    }

    public Result<Category, DomainError> ChangeName(Name changedName)
    {
        if (changedName is null)
        {
            return DomainError.Validation(Name.RequiredMessage);
        }

        if (Name == changedName)
        {
            return this; // no-op
        }

        Name = changedName;
        return this;
    }

    public override string ToString() => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Category() => Name = null!;
}
