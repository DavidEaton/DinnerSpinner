using DinnerSpinner.Api.Features.Common;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Categories.Update;

public class Validator : Validator<Request>
{
    public Validator() =>
        RuleFor(request => request.Name)
            .MustBeValueObject<Request, Name, string>(
                name => Name.Create(name).Value);
}
