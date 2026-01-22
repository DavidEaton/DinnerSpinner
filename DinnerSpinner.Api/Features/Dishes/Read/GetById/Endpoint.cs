using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Read.GetById
{
    public sealed class Endpoint(AppDbContext db)
        : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Get("/api/dishes/{id:int}");
            AllowAnonymous();
            Summary(s => s.Summary = "Get a dish by id");
        }

        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var dish = await db.Dishes
                .Include(dish => dish.Category)
                .FirstOrDefaultAsync(dish => dish.Id == request.Id, cancellationToken);

            if (dish is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            await Send.OkAsync(dish.ToGetByIdResponse(), cancellationToken);
        }
    }

}
