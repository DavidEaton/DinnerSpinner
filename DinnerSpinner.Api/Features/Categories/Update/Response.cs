namespace DinnerSpinner.Api.Features.Categories.Update;

public sealed record Response : IResponse
{
    public Contract Category { get; init; } = new();
}
