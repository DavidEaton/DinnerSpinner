using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Read.GetById
{
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
                .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

            if (category is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            var response = category.ToGetByIdResponse();
            if (response is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                return;
            }

            await Send.OkAsync(response, cancellationToken);
        }
    }

}
