namespace DinnerSpinner.Api.Features.Dishes.Update;

public sealed record Response
{
    public Contract Dish { get; init; } = new();
}
