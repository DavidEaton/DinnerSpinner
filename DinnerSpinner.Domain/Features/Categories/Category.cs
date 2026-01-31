using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Features.Common;
using Entity = DinnerSpinner.Domain.BaseClasses.Entity;

namespace DinnerSpinner.Domain.Features.Categories;

public class Category : Entity
{
    public Name Name { get; private set; } = null!;

    private Category(Name name)
    {
        Name = name;
    }

    public static Result<Category> Create(string name)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Category>(nameResult.Error);
        }

        return Result.Success(new Category(nameResult.Value));
    }


    public Result Rename(string newName)
    {
        var nameResult = Name.Create(newName);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Category>(nameResult.Error);
        }

        return Result.Success(new Category(nameResult.Value));
    }

    public override string ToString()
    {
        return Name.ToString();
    }

    // Entity Framework requires an empty constructor
    protected Category() { }
}
