using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Read.List;

public sealed class Endpoint(AppDbContext db)
    : EndpointWithoutRequest<ApiResponse<Response>>
{
    public override void Configure()
    {
        Get("/api/dishes");
        AllowAnonymous();
        Summary(summary => summary.Summary = "List dishes");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var rows = await db.Dishes
            .AsNoTracking()
            .Join(
                db.Categories.AsNoTracking(),
                d => d.CategoryId.Value,
                c => c.Id,
                (d, c) => new DishReadRow(
                    d.Id,
                    d.Name.Value,
                    d.CategoryId.Value,
                    c.Name.Value
                )
            )
            .ToListAsync(cancellationToken);

        var response = new Response
        {
            Dishes = rows
                .Select(row => row
                .ToContract())
                .ToList()
        };

        await Send.OkAsync(response, cancellationToken);
    }
}
