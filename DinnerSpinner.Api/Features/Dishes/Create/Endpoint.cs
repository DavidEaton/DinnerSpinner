using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    public class Endpoint(AppDbContext db)
        : Endpoint<Request, ApiResponse<Response>>
    {
        public override void Configure()
        {
            Post("/api/dishes");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new dish");
        }

        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();
            var categoryId = request.CategoryId;

            var category = await db.Categories.FindAsync([categoryId], cancellationToken);
            if (category is null)
            {
                await Send.NotFoundAsync("Category not found.", cancellationToken);
                return;
            }

            var exists = await db.Dishes.AnyAsync(
                d => d.Name.Value == name && d.Category.Id == categoryId,
                cancellationToken);

            if (exists)
            {
                await Send.ConflictAsync(
                    "A dish with the same name already exists in this category.",
                    cancellationToken);
                return;
            }

            var nameResult = Name.Create(name);
            
            if (nameResult.IsFailure)
            {
                await Send.ValidationAsync(nameResult.Error, cancellationToken);
                return;
            }

            var dishResult = Dish.Create(nameResult.Value, category);

            if (dishResult.IsFailure)
            {
                await Send.ValidationAsync(dishResult.Error, cancellationToken);
                return;
            }

            db.Dishes.Add(dishResult.Value);
            await db.SaveChangesAsync(cancellationToken);

            await Send.CreatedAsync(
                dishResult.Value.ToCreateResponse(),
                cancellationToken);
        }
    }
}
