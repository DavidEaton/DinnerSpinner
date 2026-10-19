using DinnerSpinner.Api.Features.Categories.Create;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Common;

public class NameValidator : Validator<Request>
{
    public NameValidator() =>
        RuleFor(request => request.Name)
            .MustBeValueObject<Request, Name, string>(
                name => Name.Create(name).Value);
}