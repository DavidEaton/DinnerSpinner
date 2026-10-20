using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Categories.Read.List;

public sealed class Endpoint(AppDbContext db)
    : EndpointWithoutRequest<ApiResponse<Response>>
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
            .AsNoTracking()
            .Select(category => new ReadRow(
                category.Id,
                category.Name.Value))
            .ToListAsync(cancellationToken);

        var response = new Response
        {
            Categories = categories
                .Select(row => row
                .ToContract())
                .ToList()
        };

        await Send.OkAsync(response, cancellationToken);
    }
}
