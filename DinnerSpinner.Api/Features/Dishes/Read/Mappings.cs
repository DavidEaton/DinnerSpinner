namespace DinnerSpinner.Api.Features.Dishes.Read;

internal static class Mappings
{
    public static Contract ToContract(this ReadRow row) =>
        new()
        {
            Id = row.Id,
            Name = row.Name,
            CategoryId = row.CategoryId,
            CategoryName = row.CategoryName
        };

    public static GetById.Response ToGetByIdResponse(this ReadRow row) =>
        new()
        {
            Dish = row.ToContract()
        };
}