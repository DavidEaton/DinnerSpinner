using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; }

    private Category(Name name)
        => Name = name;

    public static Result<Category, DomainError> Create(Name name)
    {
        return IsMissing(name)
            ? DomainError.Validation(Name.RequiredMessage, "Name")
            : new Category(name);
    }

    public Result<Category, DomainError> ChangeName(Name changedName)
    {
        if (IsMissing(changedName))
        {
            return DomainError.Validation(Name.RequiredMessage, "Name");
        }

        if (Name != changedName)
        {
            Name = changedName;
        }

        return this;
    }

    private static bool IsMissing(Name name) =>
        name is null || string.IsNullOrWhiteSpace(name.Value);

    public override string ToString()
        => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Category() =>
        Name = null!;
}
