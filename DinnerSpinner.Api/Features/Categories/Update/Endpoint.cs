using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Update
{
    public sealed class Endpoint(AppDbContext db) : Endpoint<Request, Response>
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

            var category = await db.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            if (!string.Equals(category.Name, request.Name, StringComparison.Ordinal))
            {
                category.Name = request.Name;
                await db.SaveChangesAsync(cancellationToken);
            }

            await Send.OkAsync(category.ToUpdateResponse(), cancellationToken);
        }
    }
}
