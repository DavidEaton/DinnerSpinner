namespace DinnerSpinner.Api.Features.Dishes.Read.GetById;

public sealed record Request
{
    public int Id { get; init; }
}
