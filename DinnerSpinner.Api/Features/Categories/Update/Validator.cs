using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using FluentValidation.Results;

namespace DinnerSpinner.Api.Features.Categories.Update;

public class Validator : Validator<Request>
{
    public Validator() =>
        RuleFor(request => request.Name).Custom((value, context) =>
        {
            var result = Name.Create(value);
            if (result.IsFailure)
            {
                context.AddFailure(new ValidationFailure(context.PropertyPath, result.Error.Description)
                {
                    ErrorCode = result.Error.Code
                });
            }
        });
}
