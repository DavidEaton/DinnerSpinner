using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
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
        if (string.IsNullOrWhiteSpace(request.Dish.Name))
        {
            await Send.ValidationAsync("Dish name is required.", cancellationToken);
            return;
        }

        if (request.Dish.CategoryId <= 0)
        {
            await Send.ValidationAsync("CategoryId must be a positive integer.", cancellationToken);
            return;
        }
        
        var id = Route<int>("id");
        var name = request.Dish.Name.Trim();
        var categoryId = request.Dish.CategoryId;
        var CategoryName = request.Dish.CategoryName;

        var dish = await db.Dishes
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
                 d.CategoryId.Value == categoryId,
            cancellationToken);

        if (duplicateExists)
        {
            await Send.ConflictAsync(
                "A dish with the same name already exists in this category.",
                cancellationToken);
            return;
        }

        var nameChanged =
            !string.Equals(dish.Name.Value, name, StringComparison.Ordinal);

        var categoryChanged = dish.CategoryId.Value != categoryId;

        if (nameChanged)
        {
            var newNameResult = Name.Create(name);
            if (newNameResult.IsFailure)
            {
                await Send.ValidationAsync(newNameResult.Error, cancellationToken);
                return;
            }

            var renameResult = dish.Rename(newNameResult.Value);
            if (renameResult.IsFailure)
            {
                await Send.ValidationAsync(renameResult.Error, cancellationToken);
                return;
            }
        }

        if (categoryChanged)
        {
            var newCategoryIdResult = CategoryId.Create(categoryId);
            if (newCategoryIdResult.IsFailure)
            {
                await Send.ValidationAsync(newCategoryIdResult.Error, cancellationToken);
                return;
            }

            var changeCategoryResult = dish.ChangeCategory(newCategoryIdResult.Value);
            if (changeCategoryResult.IsFailure)
            {
                await Send.ValidationAsync(changeCategoryResult.Error, cancellationToken);
                return;
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await Send.OkAsync(dish.ToUpdateResponse(CategoryName), cancellationToken);
    }
}
