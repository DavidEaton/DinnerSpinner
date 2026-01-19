namespace DinnerSpinner.Api.Features.Dishes.Create
{
    public class Request
    {
        public string Name { get; init; } = string.Empty;
        public int CategoryId { get; init; } = 0;

    }
}
