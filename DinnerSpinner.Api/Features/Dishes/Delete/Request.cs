namespace DinnerSpinner.Api.Features.Dishes.Delete;

public sealed record Request
{
    public int Id { get; init; }
}