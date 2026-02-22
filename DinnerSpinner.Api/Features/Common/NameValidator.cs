using DinnerSpinner.Api.Features.Categories.Create;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Api.Features.Common;

public class NameValidator : AbstractValidator<Request>
{
    public NameValidator() =>
        RuleFor(request => request.Name)
            .MustBeValueObject<Request, Name, string>(
                name => Name.Create(name).Value);
}