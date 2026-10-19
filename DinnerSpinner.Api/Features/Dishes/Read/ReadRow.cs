namespace DinnerSpinner.Api.Features.Dishes.Read;

internal sealed record ReadRow(
    int Id,
    string Name,
    int CategoryId,
    string CategoryName
);