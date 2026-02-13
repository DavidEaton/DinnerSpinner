using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; }

    private Category(Name name)
        => Name = name;

    public static Result<Category> Create(Name name)
    {
        if (name is null || string.IsNullOrWhiteSpace(name.Value))
            return Fail(Name.RequiredMessage);

        return Result.Success(new Category(name));
    }

    public Result<Category> Rename(Name newName)
    {
        if (newName is null || string.IsNullOrWhiteSpace(newName.Value))
            return Fail(Name.RequiredMessage);

        if (Name != newName)
            Name = newName;

        return Result.Success(this);
    }

    private static Result<Category> Fail(string message) =>
        Result.Failure<Category>(message);
    
    public override string ToString()
        => Name.ToString();

    // Entity Framework requires a parameterless constructor for entity materialization
    protected Category() =>
        Name = null!;
}
