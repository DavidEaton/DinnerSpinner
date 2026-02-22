using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
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
        // How do we validate the request at this point? We have some options:
        // 1. Manually validate here in HandleAsync (not ideal, can get messy)
        // 2. Use FluentValidation to validate BEFORE HandleAsync runs (ideal, keeps validation separate from business logic)
        // 3. Use a custom middleware to validate the request before it reaches the endpoint (more complex, but can be reusable across endpoints)
        
        // FluentValidation handles request validation BEFORE HandleAsync
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
            AddError(
                property: request => request.Dish,
                errorMessage: DomainError.Validation("Dish not found.").ToString(),
                errorCode: ErrorCode.NotFound.ToString());

            ThrowIfAnyErrors();
        }

        var category = await db.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId, cancellationToken);
        if (category is null)
        {
            AddError(
                property: request => request.Dish,
                errorMessage: DomainError.Validation("Category not found.").ToString(),
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
                errorMessage: DomainError.Conflict("A dish with the same name already exists in this category.", "Name").ToString(),
                errorCode: ErrorCode.Conflict.ToString());

            ThrowIfAnyErrors();
        }

        var nameChanged =
            !string.Equals(dish!.Name.Value, trimmedName, StringComparison.Ordinal);
        if (nameChanged)
        {
            var newNameResult = Name.Create(trimmedName);
            if (newNameResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish,
                    errorMessage: DomainError.Validation($"Name is invalid: {newNameResult.Error}.", "Name").ToString(),
                    errorCode: ErrorCode.Conflict.ToString());

                ThrowIfAnyErrors();
            }

            var changeNameResult = dish.ChangeName(newNameResult.Value);
            if (changeNameResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish,
                    errorMessage: DomainError.Validation($"Name is invalid: {changeNameResult.Error}.", "Name").ToString(),
                    errorCode: ErrorCode.Conflict.ToString());

                ThrowIfAnyErrors();
            }
        }

        var categoryChanged = dish.CategoryId.Value != categoryId;
        if (categoryChanged)
        {
            var newCategoryIdResult = CategoryId.Create(categoryId);
            if (newCategoryIdResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish.CategoryId,
                    errorMessage: DomainError.Validation($"Category is invalid: {newCategoryIdResult.Error}.", "Category").ToString(),
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors();
            }

            var changeCategoryResult = dish.ChangeCategory(newCategoryIdResult.Value);
            if (changeCategoryResult.IsFailure)
            {
                AddError(
                    property: request => request.Dish.CategoryId,
                    errorMessage: DomainError.Validation($"Category is invalid: {changeCategoryResult.Error}.", "Category").ToString(),
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors();
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await Send.OkAsync(dish.ToUpdateResponse(categoryName), cancellationToken);
    }
}
