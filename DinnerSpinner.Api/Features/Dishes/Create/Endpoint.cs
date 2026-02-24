using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Dishes.Create;

public sealed class Endpoint(AppDbContext db)
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
        if (request is null || string.IsNullOrWhiteSpace(request?.Name) || request?.CategoryId <= 0)
        {
            AddError(
                property: request => request,
                errorMessage: "Category and Name are required.",
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            // emits 400 ProblemDetails (our DinnerSpinnerProblemDetails DTO)
            ThrowIfAnyErrors();
        }

        var trimmedName = request!.Name.Trim();
        var categoryId = request.CategoryId;
        var category = await db.Categories.FindAsync([categoryId], cancellationToken);
        if (category is null)
        {
            AddError(
                property: request => request.CategoryId,
                errorMessage: "Category not found.",
                severity: Severity.Error,
                errorCode: ErrorCode.NotFound.ToString());

            ThrowIfAnyErrors();
        }

        var duplicateExists = await db.Dishes.AnyAsync(
            dish =>
            dish.Name.Value == trimmedName
            &&
            dish.CategoryId.Value == categoryId,
            cancellationToken);
        if (duplicateExists)
        {
            AddError(
                property: request => request,
                errorMessage: Error.Conflict("A dish with the same name already exists in this category.", "Name").ToString(),
                errorCode: ErrorCode.Conflict.ToString(),
                severity: Severity.Error);

            ThrowIfAnyErrors();
        }

        var nameResult = Name.Create(trimmedName);
        if (nameResult.IsFailure)
        {
            AddError(
                property: request => request.Name,
                errorMessage: nameResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors();
        }

        var categoryIdResult = CategoryId.Create(categoryId);
        if (categoryIdResult.IsFailure)
        {
            AddError(
                property: request => request.CategoryId,
                errorMessage: categoryIdResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors();
        }

        var dishResult = Dish.Create(
            nameResult.Value,
            categoryIdResult.Value);
        if (dishResult.IsFailure)
        {
            AddError(
                property: request => request.ToString(), // no specific property, so use the request as a whole for context in the error message.
                errorMessage: dishResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors();
        }

        db.Dishes.Add(dishResult.Value);
        await db.SaveChangesAsync(cancellationToken);

        await Send.CreatedAsync(
            dishResult.Value.ToCreateResponse(category!.Name.Value),
            cancellationToken);
    }
}
