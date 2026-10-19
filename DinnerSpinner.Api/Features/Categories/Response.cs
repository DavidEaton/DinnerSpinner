namespace DinnerSpinner.Api.Features.Categories;

public readonly record struct Response(Contract Category) : IResponse;