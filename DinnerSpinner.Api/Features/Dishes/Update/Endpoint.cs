using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Common;
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
        var name = request.Dish.Name.Trim();
        var categoryId = request.Dish.CategoryId;
        var CategoryName = request.Dish.CategoryName;

        var dish = await db.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (dish is null)
        {
            await Send.NotFoundAsync("Dish not found.", cancellationToken);
            return;
        }

        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        if (category is null)
        {
            await Send.NotFoundAsync("Category not found.", cancellationToken);
            return;
        }

        var duplicateExists = await db.Dishes.AnyAsync(
            d => d.Id != id &&
                 d.Name.Value == name &&
                 d.Category.Id == categoryId,
            cancellationToken);

        if (duplicateExists)
        {
            await Send.ConflictAsync(
                "A dish with the same name already exists in this category.",
                cancellationToken);
            return;
        }

        var changed =
            !string.Equals(dish.Name.Value, name, StringComparison.Ordinal) ||
            dish.Category.Id != categoryId;

        if (changed)
        {
            dish.Rename(Name.Create(name).Value);
            dish.ChangeCategory(category);

            await db.SaveChangesAsync(cancellationToken);
        }

        await Send.OkAsync(dish.ToUpdateResponse(), cancellationToken);
    }
}
