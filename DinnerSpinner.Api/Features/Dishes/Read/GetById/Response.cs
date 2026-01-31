namespace DinnerSpinner.Api.Features.Dishes.Read.GetById;

public sealed record Response
{
    public Contract Dish { get; init; } = new();
}
