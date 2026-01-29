namespace DinnerSpinner.Api.Features.Categories.Read.GetById;

public sealed class Response : IResponse
{
    public Contract Category { get; init; } = new();
}
