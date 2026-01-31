namespace DinnerSpinner.Api.Features.Dishes.Create;

public sealed record Request
{
    public string Name { get; init; } = string.Empty;
    public int CategoryId { get; init; } = 0;

}
