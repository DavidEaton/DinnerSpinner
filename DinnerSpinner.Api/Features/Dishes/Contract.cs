namespace DinnerSpinner.Api.Features.Dishes
{
    public sealed class Contract
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
    }
}