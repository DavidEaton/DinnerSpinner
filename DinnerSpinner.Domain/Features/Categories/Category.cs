using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; } = null!;

    private Category(Name name)
        => Name = name;

    public static Result<Category> Create(Name name)
    {
        if (name is null)
        {
            return Result.Failure<Category>(Name.RequiredMessage);
        }

        return Result.Success(new Category(name));
    }

    public Result Rename(Name newName)
    {
        if (newName is null)
        {
            return Result.Failure<Category>(Name.RequiredMessage);
        }

        if (Name == newName)
        {
            return Result.Success(); //no-op
        }

        Name = newName;
        return Result.Success();
    }

    public override string ToString()
        => Name.ToString();

    // Entity Framework requires an empty constructor
    protected Category() { }
}
