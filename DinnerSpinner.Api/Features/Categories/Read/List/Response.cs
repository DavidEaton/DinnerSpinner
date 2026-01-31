namespace DinnerSpinner.Api.Features.Categories.Read.List;

public sealed record Response : IResponse
{
    public Contract Category { get; init; } = new();
}
