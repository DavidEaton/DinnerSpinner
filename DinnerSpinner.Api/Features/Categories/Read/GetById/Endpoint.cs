using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Read.GetById;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response?>>
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
            .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

        await Send.OkAsync(category?.ToGetByIdResponse(), cancellationToken);
    }
}
