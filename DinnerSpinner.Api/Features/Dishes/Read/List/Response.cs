using System.Collections.Generic;

namespace DinnerSpinner.Api.Features.Dishes.Read.List;

public sealed record Response
{
    public List<Contract> Dishes { get; init; } = [];
}
