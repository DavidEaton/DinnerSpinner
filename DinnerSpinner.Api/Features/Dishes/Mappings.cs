using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Api.Features.Dishes;

internal static class Mappings
{
    private static Contract ToContract(this Dish dish) => new()
    {
        Id = dish.Id,
        Name = dish.Name.Value,
        CategoryId = dish.Category?.Id ?? 0,
        CategoryName = dish.Category?.Name.Value ?? string.Empty
    };

    public static Create.Response ToCreateResponse(this Dish dish) => new()
    {
        Dish = dish.ToContract()
    };

    public static Update.Response ToUpdateResponse(this Dish dish) => new()
    {
        Dish = dish.ToContract()
    };

    public static Read.GetById.Response ToGetByIdResponse(this Dish dish) => new()
    {
        Dish = dish.ToContract()
    };

    public static Read.List.Response ToListResponse(this Dish dish) => new()
    {
        Dish = dish.ToContract()
    };
}