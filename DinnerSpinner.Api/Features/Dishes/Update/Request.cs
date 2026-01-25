namespace DinnerSpinner.Api.Features.Dishes.Update
{
    public sealed class Request
    {
        public Contract Dish { get; init; } = new();
    }
}
