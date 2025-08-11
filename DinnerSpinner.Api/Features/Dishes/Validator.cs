using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes
{
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(dish => dish.Name)
                .NotEmpty()
                .WithMessage("Please enter a dish Name")
                .MinimumLength(2)
                .WithMessage("Dish Name is too short")
                .Length(2, 1000)
                .WithMessage("Dish Name is too long");

            RuleFor(dish => dish.Category)
                .NotEmpty()
                .WithMessage("Please select a category");
        }
    }
}
