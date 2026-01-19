namespace DinnerSpinner.Api.Features.Categories
{
    internal static class Mappings
    {
        public static Create.Response ToCreateResponse(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };

        public static Read.GetById.Response? ToGetByIdResponse(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };

        public static Update.Response ToUpdateResponse(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };

        public static Read.List.Response? ToListResponse(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}