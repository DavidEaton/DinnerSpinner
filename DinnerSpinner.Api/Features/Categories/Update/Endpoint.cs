using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

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
        var id = Route<int>("id");
        var name = request.Name.Trim();

        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category is null)
        {
            await Send.NotFoundAsync("Category not found.", cancellationToken);
            return;
        }

        var duplicateExists = await db.Categories.AnyAsync(
            c => c.Id != id &&
                 c.Name.Value == name,
            cancellationToken);

        if (duplicateExists)
        {
            await Send.ConflictAsync(
                "A category with the same name already exists.",
                cancellationToken);
            return;
        }

        var changed =
            !string.Equals(category.Name.Value, name, StringComparison.Ordinal);

        if (changed)
        {
            category.Rename(Name.Create(name).Value);
            await db.SaveChangesAsync(cancellationToken);
        }
        
        await Send.OkAsync(category.ToUpdateResponse(), cancellationToken);
    }
}
