namespace DinnerSpinner.Api.Features.Categories.Update
{
public sealed class Request
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
