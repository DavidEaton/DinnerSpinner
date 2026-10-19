using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            .Select(dish => new ReadRow(
                dish.Id,
                dish.Name.Value,
                dish.Category.Id,
                dish.Category.Name.Value))
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