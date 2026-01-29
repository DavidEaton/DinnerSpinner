namespace DinnerSpinner.Api.Features.Dishes.Update;

public sealed class Response : IResponse
{
    public Contract Dish { get; init; } = new();
}
