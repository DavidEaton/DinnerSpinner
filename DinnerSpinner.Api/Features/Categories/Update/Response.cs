namespace DinnerSpinner.Api.Features.Categories.Update;

public sealed class Response : IResponse
{
    public Contract Category { get; init; } = new();
}
