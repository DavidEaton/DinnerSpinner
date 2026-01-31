namespace DinnerSpinner.Api.Features.Categories;

public sealed record Contract
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}