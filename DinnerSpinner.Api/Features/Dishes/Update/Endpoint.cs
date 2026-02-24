using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        // FluentValidation handles request validation BEFORE HandleAsync, eliminating the need for manual validation logic here and keeping our endpoint focused.
        var trimmedName = request.Dish.Name.Trim();
        var categoryId = request.Dish.CategoryId;

        var dish = await db.Dishes
            .FirstOrDefaultAsync(dish => dish.Id == request.Dish.Id, cancellationToken);
        if (dish is null)
        {
            AddError(
                property: request => request.Dish,
                errorMessage: Errors.NotFound(request.Dish.Id).ToString(),
                errorCode: ErrorCode.NotFound.ToString());

            ThrowIfAnyErrors();
        }

        var category = await db.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId, cancellationToken);
        if (category is null)
        {
            AddError(
                property: request => request.Dish,
                errorMessage: Errors.ValidCategoryRequired().ToString(),
                errorCode: ErrorCode.NotFound.ToString());

            ThrowIfAnyErrors();
        }

        var categoryName = category!.Name.Value;

        var duplicateExists = await db.Dishes.AnyAsync(
            dish => dish.Id != request.Dish.Id &&
                 dish.Name.Value == trimmedName &&
                 dish.CategoryId.Value == categoryId,
            cancellationToken);
        if (duplicateExists)
        {
            AddError(
                property: request => request.Dish,
                errorMessage: Errors.Conflict(request.Dish.Name, request.Dish.CategoryName).ToString(),
                errorCode: ErrorCode.Conflict.ToString());

            ThrowIfAnyErrors();
        }

        if (dish!.Name.Value == trimmedName)
        {
            var newNameResult = Name.Create(trimmedName);
            if (newNameResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish,
                    errorMessage: Errors.ValidNameRequired().ToString(),
                    errorCode: ErrorCode.Conflict.ToString());

                ThrowIfAnyErrors();
            }

            var changeNameResult = dish.ChangeName(newNameResult.Value);
            if (changeNameResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish,
                    errorMessage: Errors.ValidNameRequired().ToString(),
                    errorCode: ErrorCode.Conflict.ToString());

                ThrowIfAnyErrors();
            }
        }

        if (dish.CategoryId.Value != categoryId)
        {
            var newCategoryIdResult = CategoryId.Create(categoryId);
            if (newCategoryIdResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish.CategoryId,
                    errorMessage: Errors.ValidCategoryRequired().ToString(),
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors();
            }

            var changeCategoryResult = dish.ChangeCategory(newCategoryIdResult.Value);
            if (changeCategoryResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish.CategoryId,
                    errorMessage: Errors.ValidCategoryRequired().ToString(),
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors();
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await Send.OkAsync(dish.ToUpdateResponse(categoryName), cancellationToken);
    }
}
