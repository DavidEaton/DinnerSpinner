namespace DinnerSpinner.Api.Features.Categories.Create;

public sealed class Response : IResponse
{
    public Contract Category { get; init; } = new();
}
