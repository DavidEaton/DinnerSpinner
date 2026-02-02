using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Api.Features.Dishes;

internal static class Mappings
{
    public static Contract ToContract(
        this Dish dish, string categoryName) => new()
    {
        Id = dish.Id,
        Name = dish.Name.Value,
        CategoryId = dish.CategoryId.Value,
        CategoryName = categoryName
    };

    public static Create.Response ToCreateResponse(
        this Dish dish, string categoryName) => new()
    {
        Dish = dish.ToContract(categoryName)
    };

    public static Update.Response ToUpdateResponse(
        this Dish dish, string categoryName) => new()
    {
        Dish = dish.ToContract(categoryName)
    };
}