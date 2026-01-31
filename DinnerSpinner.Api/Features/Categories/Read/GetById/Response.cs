namespace DinnerSpinner.Api.Features.Categories.Read.GetById;

public sealed record Response : IResponse
{
    public Contract Category { get; init; } = new();
}
