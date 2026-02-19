using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Categories.Update;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
{
    public override void Configure()
    {
        Put("/api/categories/{id:int}");
        Validator<Validator>();
        AllowAnonymous();
        Summary(s => s.Summary = "Update a category by id");
    }
    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var id = request.Id;

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

        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category is null)
        {
            ThrowError(
                message: "Category not found.",
                errorCode: ErrorCode.NotFound.ToString(),
                statusCode: StatusCodes.Status404NotFound);
        }

        var exists = await db.Categories.AnyAsync(
            category =>
            category.Id != id
            &&
            category.Name.Value == trimmedName,
            cancellationToken);
        if (exists)
        {
            ThrowError(
                message: "A category with the same name already exists.",
                errorCode: ErrorCode.Conflict.ToString(),
                statusCode: StatusCodes.Status409Conflict);
        }

        var changed =
            !string.Equals(category.Name.Value, trimmedName, StringComparison.Ordinal);
        if (changed)
        {
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

            category.ChangeName(nameResult.Value);
            await db.SaveChangesAsync(cancellationToken);
        }

        await Send.OkAsync(category.ToUpdateResponse(), cancellationToken);
    }
}
