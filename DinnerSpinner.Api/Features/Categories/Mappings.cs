using DinnerSpinner.Domain.Features.Categories;

namespace DinnerSpinner.Api.Features.Categories;

internal static class Mappings
{
    private static Contract ToContract(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name.Value
    };

    public static Create.Response ToCreateResponse(this Category category) => new()
    {
        Category = category.ToContract()
    };

    public static Read.GetById.Response? ToGetByIdResponse(this Category category) => new()
    {
        Category = category.ToContract()
    };

    public static Update.Response ToUpdateResponse(this Category category) => new()
    {
        Category = category.ToContract()
    };

    public static Read.List.Response ToListResponse(this Category category) => new()
    {
        Category = category.ToContract()
    };
}