using DinnerSpinner.Api.Data;
using DinnerSpinner.Api.Domain;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    internal class Endpoint(AppDbContext db) : Endpoint<Request, Response, Mapper>
    {
        public override void Configure()
        {
            Post("/api/dishes/create");
            AllowAnonymous();
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

            await Send.CreatedAtAsync<GetById.Endpoint>(
                routeValues: new { id = entity.Id },
                responseBody: entity.ToCreateResponse(),
                cancellation: cancellationToken);
        }
    }
}
