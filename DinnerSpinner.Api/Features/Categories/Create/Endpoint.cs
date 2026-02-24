using System.Threading;
using System.Threading.Tasks;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Create;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
{
    public override void Configure()
    {
        Post("/api/categories");
        Description(builder => builder
            .ProducesProblemDetails(400, "application/json+problem")
            .ProducesProblemDetails(409, "application/json+problem")
            .Produces<Response>(201, "application/json")
            .ProducesProblemFE<InternalErrorResponse>(500)); 
        Validator<Validator>();
        AllowAnonymous();
        Summary(s => s.Summary = "Create a new category");
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var nameTrimmed = request!.Name.Trim();

        var exists = await db.Categories.AnyAsync(
            category => category.Name.Value == nameTrimmed,
            cancellationToken);
        if (exists)
        {
            AddError(
                property: request => request,
                errorMessage: Error.Conflict("Category already exists.", nameof(request.Name)).ToString(),
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

        var categoryResult = Category.Create(nameResult.Value);
        if (categoryResult.IsFailure)
        {
            AddError(
                property: request => request,
                errorMessage: categoryResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors(StatusCodes.Status400BadRequest);
        }

        db.Categories.Add(categoryResult.Value);
        await db.SaveChangesAsync(cancellationToken);

        await Send.CreatedAsync(
            categoryResult.Value.ToCreateResponse(),
            cancellationToken);
    }
}
