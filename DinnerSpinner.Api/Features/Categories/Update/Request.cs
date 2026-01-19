using DinnerSpinner.Api.Features.Categories;

namespace DinnerSpinner.Api.Features.Categories.Update
{
    public class Request
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
