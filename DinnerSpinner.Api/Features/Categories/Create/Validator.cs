using FastEndpoints;

namespace DinnerSpinner.Api.Features.Categories.Create
{
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(category => category.Name)
                .NotEmpty()
                .WithMessage("Please enter a category Name")
                .MinimumLength(2)
                .WithMessage("Category Name is too short")
                .Length(2, 1000)
                .WithMessage("Category Name is too long");
        }
    }
}
