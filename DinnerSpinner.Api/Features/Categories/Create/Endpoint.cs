using System.Threading;
using System.Threading.Tasks;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Errors;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Create;

public class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
{
    public override void Configure()
    {
        Post("/api/categories");
        Validator<Validator>();
        AllowAnonymous();
        Summary(s => s.Summary = "Create a new category");
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        var exists = await db.Categories.AnyAsync(
            c => c.Name.Value == name,
            cancellationToken);

        if (exists)
        {
            ThrowError(
                message: "Category already exists.",
                errorCode: ErrorCode.Conflict.ToString(),
                statusCode: StatusCodes.Status409Conflict);
        }

        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
        {
            AddError(
                property: request => request.Name,
                errorMessage: nameResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors();
        }

        var categoryResult = Category.Create(nameResult.Value);
        if (categoryResult.IsFailure)
        {
            AddError(
                property: request => request.Name,
                errorMessage: categoryResult.Error.ToString(),
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            ThrowIfAnyErrors();
        }

        db.Categories.Add(categoryResult.Value);
        await db.SaveChangesAsync(cancellationToken);

        await Send.CreatedAsync(
            categoryResult.Value.ToCreateResponse(),
            cancellationToken);
    }
}
