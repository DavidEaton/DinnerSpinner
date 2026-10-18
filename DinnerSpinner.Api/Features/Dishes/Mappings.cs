using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Api.Features.Dishes;

internal static class Mappings
{
    public static Contract ToContract(this Dish dish) =>
        new()
        {
            Id = dish.Id,
            Name = dish.Name.Value,
            CategoryId = dish.Category.Id,
            CategoryName = dish.Category.Name.Value
        };

    public static Create.Response ToCreateResponse(this Dish dish) =>
        new()
        {
            Dish = dish.ToContract()
        };

    public static Update.Response ToUpdateResponse(this Dish dish)
        {
            // 1. Build contract
            var contract = dish.ToContract();

            // 2. Build response
            var response = new Update.Response
            {
                Dish = contract
            };
            // 3. Return response
            return response;
        }
}