using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Api.Features.Dishes;

internal static class Mappings
{
    public static Contract ToContract(
        this Dish dish,
        string categoryName)
    {
        return new Contract
        {
            Id = dish.Id,
            Name = dish.Name.Value,
            CategoryId = dish.CategoryId.Value,
            CategoryName = categoryName
        };
    }

    public static Create.Response ToCreateResponse(
        this Dish dish,
        string categoryName)
    {
        return new Create.Response
        {
            Dish = dish.ToContract(categoryName)
        };
    }

    public static Update.Response ToUpdateResponse(
        this Dish dish,
        string categoryName)
    {
        // 1. Build contract
        var contract = dish.ToContract(categoryName);

        // 2. Build response
        var response = new Update.Response
        {
            Dish = contract
        };
        // 3. Return response
        return response;
    }
}