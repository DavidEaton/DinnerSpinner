namespace DinnerSpinner.Api.Features.Dishes.Update;

public sealed record Response : IResponse
{
    public Contract Dish { get; init; } = new();
}
