namespace DinnerSpinner.Api.Features.Categories.Update;

public sealed record Request
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
