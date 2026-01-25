using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes.Update
{
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.Dish.Name)
                .NotEmpty()
                .WithMessage("Please enter a dish Name")
                .MinimumLength(2)
                .WithMessage("Dish Name is too short")
                .Length(2, 1000)
                .WithMessage("Dish Name is too long");

            RuleFor(request => request.Dish.CategoryId)
                .GreaterThan(0)
                .WithMessage("Please select a category");
        }
    }
}