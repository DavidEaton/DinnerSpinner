namespace DinnerSpinner.Api.Features.Dishes.Read.List;

public sealed class Response: IResponse
{
    public Contract Dish { get; init; } = new();
}
