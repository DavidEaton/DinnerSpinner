namespace DinnerSpinner.Api.Features.Dishes.Read;

internal sealed record DishReadRow(
    int Id,
    string Name,
    int CategoryId,
    string CategoryName
);