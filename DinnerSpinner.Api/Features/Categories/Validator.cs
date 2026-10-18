using DinnerSpinner.Api.Features.Categories.Create;
using DinnerSpinner.Api.Features.Common;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using FastEndpoints;
using Result = DinnerSpinner.Domain.Abstractions.Result;

namespace DinnerSpinner.Api.Features.Categories;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(category => category)
            .MustBeEntity(
                category =>
                {
                    if (category.Name is null)
                    {
                        return Result.Failure<Category>(Errors.ValidCategoryRequired());
                    }

                    var nameResult = Name.Create(category.Name);
                    if (nameResult.IsFailure)
                    {
                        return Result.Failure<Category>(nameResult.Error);
                    }

                    var categoryResult = Category.Create(nameResult.Value);
                    return categoryResult.IsSuccess
                        ? Result.Success(categoryResult.Value)
                        : Result.Failure<Category>(categoryResult.Error);
                });
    }
}
