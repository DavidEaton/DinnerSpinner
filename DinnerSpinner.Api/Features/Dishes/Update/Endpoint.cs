using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Update
{
    public sealed class Endpoint(AppDbContext db) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Put("/api/dishes/{id:int}");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Update a dish by id");
        }
        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var id = Route<int>("id");

            var dish = await db.Dishes.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            if (dish is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            var categoryExists = await db.Categories
                .AnyAsync(
                    category => category.Id == request.Category.Id,
                    cancellationToken
                );
            if (!categoryExists)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            dish.Name = request.Name;
            dish.Category = request.Category;

            await db.SaveChangesAsync(cancellationToken);
            await db.Entry(dish).Reference(d => d.Category).LoadAsync(cancellationToken);

            await Send.OkAsync(dish.ToUpdateResponse(), cancellationToken);
        }
    }
}
