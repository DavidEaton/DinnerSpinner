using DinnerSpinner.Api.Domain;

namespace DinnerSpinner.Api.Features.Dishes
{
    public sealed class Request
    {

        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; } = new Category();
    }

    public sealed class Response
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Category Category { get; set; } = new Category();
    }
}
