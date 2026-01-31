namespace DinnerSpinner.Api.Features.Categories.Create;

public sealed record Request
{
    public string Name { get; init; } = string.Empty;
}
