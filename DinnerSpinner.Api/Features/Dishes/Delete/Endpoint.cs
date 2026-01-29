using DinnerSpinner.Api.Data;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes.Delete;

public class Delete(AppDbContext db)
    : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("/api/dishes/{id:int}");
        AllowAnonymous();
        Summary(s => s.Summary = "Delete a dish by id");
    }
    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var dish = await db.Dishes.FindAsync([request.Id], cancellationToken);

        if (dish is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        db.Dishes.Remove(dish);

        await db.SaveChangesAsync(cancellationToken);
        await Send.NoContentAsync(cancellationToken);
    }
}
