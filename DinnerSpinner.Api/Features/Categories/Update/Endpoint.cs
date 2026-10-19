using CSharpFunctionalExtensions;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
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
        // FastEndpoints has validated the request before invoking this handler.
        var name = Name.Create(request.Name).Value;

        var id = Route<int>("id");
        var category = await db.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
        if (category is null)
        {
            await Send.ResultAsync(Error.NotFound(
                "Category.Errors.NotFound",
                $"Category with Id '{id}' was not found.").ToProblemDetails());
            return;
        }

        var duplicateExists = await db.Categories.AnyAsync(
            other => other.Id != id && other.Name.Value == name.Value,
            cancellationToken);
        if (duplicateExists)
        {
            await Send.ResultAsync(Error.Conflict(
                "Category.Errors.Conflict",
                $"A category named '{name.Value}' already exists.").ToProblemDetails());
            return;
        }

        var changed = category.Name != name;
        await category.ChangeName(name).Match(
            async updated =>
            {
                if (changed)
                    await db.SaveChangesAsync(cancellationToken);

                await Send.OkAsync(updated.ToUpdateResponse(), cancellationToken);
            },
            error => Send.ResultAsync(error.ToProblemDetails()));
    }
}
