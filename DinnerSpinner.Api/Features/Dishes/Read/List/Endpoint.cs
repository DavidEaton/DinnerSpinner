using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Read.List
{
    public sealed class Endpoint(AppDbContext db)
        : EndpointWithoutRequest<List<Response?>>
    {
        public override void Configure()
        {
            Get("/api/dishes");
            AllowAnonymous();
            Summary(summary => summary.Summary = "List dishes");
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var dishes = await db.Dishes
                .Select(dish => dish.ToListResponse())
                .ToListAsync(cancellationToken);
            await Send.OkAsync(dishes, cancellationToken);
        }
    }
}
