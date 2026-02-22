using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
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
        var nameTrimmed = request!.Name.Trim();
        var id = request!.Id;

        var categoryFromDatabase = await db.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
        if (categoryFromDatabase is null)
        {
            AddError(
                property: request => request.Id,
                errorMessage: "Category not found.",
                severity: Severity.Error,
                errorCode: ErrorCode.NotFound.ToString());

            ThrowIfAnyErrors(StatusCodes.Status404NotFound);
        }

        var duplicateExists = await db.Categories.AnyAsync(
            category =>
            category.Id != id
            &&
            category.Name.Value == nameTrimmed,
            cancellationToken);
        if (duplicateExists)
        {
            AddError(
                property: request => request,
                errorMessage: $"A category named '{nameTrimmed}' already exists.",
                errorCode: ErrorCode.Conflict.ToString(),
                severity: Severity.Error);

            ThrowIfAnyErrors(StatusCodes.Status409Conflict);
        }

        var nameResult = Name.Create(nameTrimmed);
        if (nameResult.IsFailure)
        {
            AddError(
                property: request => request.Name,
                errorMessage: nameResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors(StatusCodes.Status400BadRequest);
        }

        if (categoryFromDatabase!.Name != nameResult.Value)
        {
            var changeNameResult = categoryFromDatabase.ChangeName(nameResult.Value);
            if (changeNameResult.IsFailure)
            {
                AddError(
                    property: request => request.Name,
                    errorMessage: changeNameResult.Error.ToString(),
                    severity: Severity.Error,
                    errorCode: ErrorCode.Validation.ToString());

                ThrowIfAnyErrors(StatusCodes.Status400BadRequest);
            }
            
            await db.SaveChangesAsync(cancellationToken);
        }

        await Send.OkAsync(categoryFromDatabase!.ToUpdateResponse(), cancellationToken);
    }
}
