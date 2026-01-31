namespace DinnerSpinner.Api.Features.Dishes.Update;

public sealed record Request
{
    public Contract Dish { get; init; } = new();
}
