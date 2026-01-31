namespace DinnerSpinner.Api.Features.Categories.Create;

public sealed record Response : IResponse
{
    public Contract Category { get; init; } = new();
}
