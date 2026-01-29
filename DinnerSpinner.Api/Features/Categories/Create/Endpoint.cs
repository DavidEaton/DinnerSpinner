using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
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
            await Send.ConflictAsync(
                "Category already exists.",
                cancellationToken);
            return;
        }

        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
        {
            await Send.ValidationAsync(nameResult.Error, cancellationToken);
            return;
        }

        var categoryResult = Category.Create(nameResult.Value);
        if (categoryResult.IsFailure)
        {
            await Send.ValidationAsync(categoryResult.Error, cancellationToken);
            return;
        }

        db.Categories.Add(categoryResult.Value);
        await db.SaveChangesAsync(cancellationToken);

        await Send.CreatedAsync(
            categoryResult.Value.ToCreateResponse(),
            cancellationToken);
    }
}
