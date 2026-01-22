namespace DinnerSpinner.Api.Features.Dishes
{
    internal static class Mappings
    {
        public static Create.Response ToCreateResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            CategoryId = dish.Category?.Id ?? 0,
            CategoryName = dish.Category?.Name ?? string.Empty
        };

        public static Read.GetById.Response ToGetByIdResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            Category = dish.Category?.Name ?? string.Empty
        };

        public static Update.Response ToUpdateResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            CategoryId = dish.Category?.Id ?? 0,
            CategoryName = dish.Category?.Name ?? string.Empty
        };

        public static Read.List.Response ToListResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            Category = dish.Category?.Name ?? string.Empty
        };
    }
}