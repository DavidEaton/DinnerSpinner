namespace DinnerSpinner.Api.Features.Dishes.Read;

internal static class Mappings
{
    public static Contract ToContract(
        this DishReadRow row) => new()
    {
        Id = row.Id,
        Name = row.Name,
        CategoryId = row.CategoryId,
        CategoryName = row.CategoryName
    };

    public static GetById.Response ToGetByIdResponse(
        this DishReadRow row) => new()
    {
        Dish = row.ToContract()
    };
}