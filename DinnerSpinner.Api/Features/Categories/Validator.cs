using DinnerSpinner.Api.Features.Categories.Create;
using DinnerSpinner.Api.Features.Common;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using FluentValidation.Validators;
using Result = CSharpFunctionalExtensions.Result;

namespace DinnerSpinner.Api.Features.Categories;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(category => category.Name)
            .SetValidator((IPropertyValidator<Request, string>)new NameValidator());

        RuleFor(category => category)
            .MustBeEntity(
                category =>
                {
                    if (category.Name is null)
                    {
                        return Result.Failure<Category>(Name.RequiredMessage);
                    }
                    
                    var nameResult = Name.Create(category.Name);
                    if (nameResult.IsFailure)
                    {
                        return Result.Failure<Category>(nameResult.Error.ToString());
                    }
                    
                    var categoryResult = Category.Create(nameResult.Value);
                    return categoryResult.IsSuccess
                        ? Result.Success(categoryResult.Value)
                        : Result.Failure<Category>(categoryResult.Error.ToString());
                });
    }
}
