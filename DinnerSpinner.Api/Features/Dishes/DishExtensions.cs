namespace DinnerSpinner.Api.Features.Dishes
{
    internal static class DishExtensions
    {
        public static Create.Response ToCreateResponse(this Dish dish)
        {
            return new Create.Response
            {
                Id = dish.Id,
                Name = dish.Name,
                Category = dish.Category?.Name ?? string.Empty
            };
        }

        public static GetById.Response? ToGetByIdResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            Category = dish.Category?.Name ?? string.Empty
        };

        public static Update.Response ToUpdateResponse(this Dish dish)
        {
            return new Update.Response
            {
                Id = dish.Id,
                Name = dish.Name,
                Category = dish.Category?.Name ?? string.Empty
            };
        }

        public static List.Response? ToListResponse(this Dish dish) => new()
        {
            Id = dish.Id,
            Name = dish.Name,
            Category = dish.Category?.Name ?? string.Empty
        };
    }
}