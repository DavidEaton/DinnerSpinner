using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    internal class CreateDish(AppDbContext db) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/api/dishes/create");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new dish");
        }
        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var category = await db
                .Categories
                .FirstOrDefaultAsync(
                    category => category.Id == request.Category.Id,
                    cancellationToken);

            if (category is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            var entity = new Dish
            {
                Name = request.Name,
                Category = category
            };

            db.Dishes.Add(entity);
            await db.SaveChangesAsync(cancellationToken);

            await Send.CreatedAtAsync<GetById.GetById>(
                routeValues: new { id = entity.Id },
                responseBody: entity.ToCreateResponse(),
                cancellation: cancellationToken);
        }
    }
}
