using DinnerSpinner.Api.Data;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    public class Endpoint(AppDbContext db)
        : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/api/dishes/create");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new dish");
        }
        public override async Task HandleAsync(Request request, CancellationToken token)
        {
            var category = await db.Categories.FindAsync([request.CategoryId], token);
            if (category is null)
            {
                await Send.NotFoundAsync(token);
                return;
            }

            var dish = new Dish
            {
                Name = request.Name,
                Category = category
            };

            db.Dishes.Add(dish);
            await db.SaveChangesAsync(token);

            await Send.CreatedAtAsync<Read.GetById.Endpoint>(
                routeValues: new { id = dish.Id },
                responseBody: dish.ToCreateResponse(),
                cancellation: token);
        }
    }
}
