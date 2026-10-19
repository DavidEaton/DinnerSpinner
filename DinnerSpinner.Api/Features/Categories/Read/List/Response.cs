using System.Collections.Generic;

namespace DinnerSpinner.Api.Features.Categories.Read.List;

public readonly record struct Response(List<Contract> Categories);