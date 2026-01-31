namespace DinnerSpinner.Api.Features.Dishes.Read.List;

public sealed record Response: IResponse
{
    public Contract Dish { get; init; } = new();
}
