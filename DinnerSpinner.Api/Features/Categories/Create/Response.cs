namespace DinnerSpinner.Api.Features.Categories.Create;

public readonly record struct Response(Contract Category) : IResponse;
