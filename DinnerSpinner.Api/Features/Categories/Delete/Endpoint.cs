using DinnerSpinner.Api.Data;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Categories.Delete
{
    public class Delete(AppDbContext db)
        : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Delete("/api/categories/{id:int}");
            AllowAnonymous();
            Summary(s => s.Summary = "Delete a category by id");
        }
        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var category = await db.Categories.FindAsync([request.Id], cancellationToken);

            if (category is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            db.Categories.Remove(category);

            await db.SaveChangesAsync(cancellationToken);
            await Send.NoContentAsync(cancellationToken);
        }
    }
}
