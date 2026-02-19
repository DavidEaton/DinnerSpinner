using CSharpFunctionalExtensions;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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
            if (request.CategoryId <= 0)
            {
                AddError(
                    property: request => request.CategoryId,
                    errorMessage: "CategoryId must be a positive integer.",
                    severity: Severity.Error,
                    errorCode: ErrorCode.Validation.ToString());

                // emits 400 ProblemDetails (our DinnerSpinnerProblemDetails DTO)
                ThrowIfAnyErrors();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                AddError(
                    property: request => request.Name,
                    errorMessage: "Name is required.",
                    severity: Severity.Error,
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors();
            }

            var trimmedName = request.Name.Trim();
            var categoryId = request.CategoryId;
            var category = await db.Categories.FindAsync([categoryId], cancellationToken);
            if (category is null)
            {
                ThrowError(
                    message: "Category not found.",
                    errorCode: ErrorCode.NotFound.ToString(),
                    statusCode: StatusCodes.Status404NotFound);
            }

            var exists = await db.Dishes.AnyAsync(
                dish =>
                dish.Name.Value == trimmedName
                &&
                dish.CategoryId.Value == categoryId,
                cancellationToken);
            if (exists)
            {
                ThrowError(
                    message: "A dish with the same name already exists in this category.",
                    errorCode: ErrorCode.Conflict.ToString(),
                    statusCode: StatusCodes.Status409Conflict);
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
                dishResult.Value.ToCreateResponse(category.Name.Value),
                cancellationToken);
        }
    }
}
