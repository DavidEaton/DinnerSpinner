namespace DinnerSpinner.Api.Features.Categories.Read;

internal static class Mappings
{
    public static Contract ToContract(this ReadRow row) =>
        new()
        {
            Id = row.Id,
            Name = row.Name
        };

    public static GetById.Response ToGetByIdResponse(this ReadRow row) =>
        new()
        {
            Category = row.ToContract()
        };
}