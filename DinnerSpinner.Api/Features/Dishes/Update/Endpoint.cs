using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
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
        // FluentValidation handles request validation before HandleAsync 
        // if (request.Id <= 0)
        // {
        //     ThrowValidationError();
        // }

        var trimmedName = request.Dish.Name.Trim();
        var categoryId = request.Dish.CategoryId;

        var dish = await db.Dishes
            .FirstOrDefaultAsync(dish => dish.Id == request.Dish.Id, cancellationToken);

        if (dish is null)
        {
            ThrowError(
                message: "Dish not found.",
                errorCode: ErrorCode.NotFound.ToString(),
                statusCode: StatusCodes.Status404NotFound);
        }

        var category = await db.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId, cancellationToken);

        if (category is null)
        {
            ThrowError(
                message: "Category not found.",
                errorCode: ErrorCode.NotFound.ToString(),
                statusCode: StatusCodes.Status404NotFound);
        }

        var categoryName = category.Name.Value;
        var duplicateExists = await db.Dishes.AnyAsync(
            dish => dish.Id != request.Dish.Id &&
                 dish.Name.Value == trimmedName &&
                 dish.CategoryId.Value == categoryId,
            cancellationToken);

        if (duplicateExists)
        {
            ThrowError(
                message: "A dish with the same name already exists in this category.",
                errorCode: ErrorCode.Conflict.ToString(),
                statusCode: StatusCodes.Status409Conflict);
        }

        var nameChanged =
            !string.Equals(dish.Name.Value, trimmedName, StringComparison.Ordinal);
        if (nameChanged)
        {
            var newNameResult = Name.Create(trimmedName);
            if (newNameResult.IsFailure)
            {
                ThrowError(
                    message: $"Failed to create Name: {newNameResult.Value}.",
                    errorCode: ErrorCode.Validation.ToString(),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var changeNameResult = dish.ChangeName(newNameResult.Value);
            if (changeNameResult.IsFailure)
            {
                ThrowError(
                    message: $"Failed to update Name: {changeNameResult.Error}.",
                    errorCode: ErrorCode.Validation.ToString(),
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }

        var categoryChanged = dish.CategoryId.Value != categoryId;
        if (categoryChanged)
        {
            var newCategoryIdResult = CategoryId.Create(categoryId);
            if (newCategoryIdResult.IsFailure)
            {
                ThrowError(
                    message: $"Failed to update Category: {newCategoryIdResult.Error}.",
                    errorCode: ErrorCode.Validation.ToString(),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var changeCategoryResult = dish.ChangeCategory(newCategoryIdResult.Value);
            if (changeCategoryResult.IsFailure)
            {
                ThrowError(
                    message: $"Failed to update Category: {changeCategoryResult.Error}.",
                    errorCode: ErrorCode.Validation.ToString(),
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await Send.OkAsync(dish.ToUpdateResponse(categoryName), cancellationToken);
    }
}
