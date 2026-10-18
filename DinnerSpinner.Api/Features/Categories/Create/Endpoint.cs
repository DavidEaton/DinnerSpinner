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
        // No request validation logic in the endpoint handler. A FluentValidation validator (Validator.cs) invokes the domain objects Create factory method, which validates its invariants. Our endpoint handlers remain clean and focused on handling requests, while our validators are responsible for running  all validation rules/domain invariants via our domain objects, which contain those rules/invariants. Many thanks to Vladimir Khorikov for this approach, which he describes in his Pluralsight course "FluentValidation Fundamentals" (https://www.pluralsight.com/courses/fluentvalidation-fundamentals).

        var exists = await db.Categories.AnyAsync(
            category => category.Name.Value == request.Name.Trim(),
            cancellationToken);
        if (exists)
        {
            AddError(
                property: request => request,
                errorMessage: Error.Conflict("Category already exists.", nameof(request.Name)).ToString(),
                errorCode: ErrorCode.Conflict.ToString(),
                severity: Severity.Error);
            // Don't trow exception here; instead, we add an error to the response and then check if any errors were added. This allows us to return multiple validation errors in a single response, if necessary. If we were to throw an exception here, it would short-circuit the request handling and we wouldn't be able to return any additional validation errors that may have been added.
            // ThrowIfAnyErrors(StatusCodes.Status409Conflict);
        }

        var category = Category.Create(
            Name.Create(
                request.Name.Trim())
                .Value)
            .Value;

        db.Categories.Add(category);
        await db.SaveChangesAsync(cancellationToken);

        await Send.CreatedAsync(
            category.ToCreateResponse(),
            cancellationToken);
    }
}
