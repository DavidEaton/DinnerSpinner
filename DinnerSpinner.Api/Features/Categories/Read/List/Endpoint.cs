using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Categories.Read.List;

public sealed class Endpoint(AppDbContext db)
    : EndpointWithoutRequest<ApiResponse<List<Response>>>
{
    public override void Configure()
    {
        Get("/api/categories");
        AllowAnonymous();
        Summary(summary => summary.Summary = "List categories");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var categories = await db.Categories
            .Select(category => category.ToListResponse())
            .ToListAsync(cancellationToken);

        await Send.OkAsync(categories, cancellationToken);
    }
}
