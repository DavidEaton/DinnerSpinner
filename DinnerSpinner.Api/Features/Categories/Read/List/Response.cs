namespace DinnerSpinner.Api.Features.Categories.Read.List;

public sealed class Response : IResponse
{
    public Contract Category { get; init; } = new();
}
