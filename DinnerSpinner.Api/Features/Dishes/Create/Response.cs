namespace DinnerSpinner.Api.Features.Dishes.Create;

public sealed record Response
{
    public Contract Dish { get; init; } = new();
}