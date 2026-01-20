using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Dishes.Update;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
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

        var dish = await db.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (dish is null)
        {
            await Send.NotFoundAsync("Dish not found.", cancellationToken);
            return;
        }

        var categoryId = request.Category.Id;

        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        if (category is null)
        {
            await Send.NotFoundAsync("Category of dish not found.", cancellationToken);
            return;
        }

        var changed =
            !string.Equals(dish.Name, request.Name, StringComparison.Ordinal) ||
            dish.Category.Id != categoryId;

        if (changed)
        {
            dish.Name = request.Name.Trim();
            dish.Category = category;

            await db.SaveChangesAsync(cancellationToken);
        }

        await Send.OkAsync(dish.ToUpdateResponse(), cancellationToken);
    }
}
