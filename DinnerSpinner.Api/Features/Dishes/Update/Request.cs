namespace DinnerSpinner.Api.Features.Dishes.Update
{
    public sealed class Request
    {
        public string Name { get; init; } = string.Empty;
        public int CategoryId { get; init; }
    }
}
