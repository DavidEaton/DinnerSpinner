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
            .Join(
                db.Categories.AsNoTracking(),
                dish => dish.CategoryId.Value,
                category => category.Id,
                (dish, category) => new DishReadRow(
                    dish.Id,
                    dish.Name.Value,
                    dish.CategoryId.Value,
                    category.Name.Value
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