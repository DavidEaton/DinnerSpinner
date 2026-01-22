namespace DinnerSpinner.Api.Features.Dishes.Read.List
{
    public sealed class Response
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
    }
}
