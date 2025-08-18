using DinnerSpinner.Api.Features.Categories;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    public class Request
    {
        public string Name { get; init; } = string.Empty;
        public Category Category { get; init; } = new();

    }
}
