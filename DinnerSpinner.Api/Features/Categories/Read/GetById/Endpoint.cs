using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Categories.Read.GetById;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/categories/{id:int}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a category by id");
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var category = await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        await Send.OkAsync(category.ToGetByIdResponse(), cancellationToken);
    }
}
