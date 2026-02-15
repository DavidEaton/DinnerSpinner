using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinnerSpinner.Api.Features.Dishes.Read.GetById;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
{
    public override void Configure()
    {
        Get("/api/dishes/{id:int}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get a dish by id");
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var id = Route<int>("id");

        var row = await db.Dishes
            .AsNoTracking()
            .Join(
                db.Categories.AsNoTracking(),
                dish => dish.CategoryId.Value,
                category => category.Id,
                (dish, category) => new DishReadRow(
                    dish.Id,
                    dish.Name.Value,
                    dish.CategoryId.Value,
                    category.Name.Value
                )
            )
            .SingleOrDefaultAsync(row => row.Id == id, cancellationToken);

        if (row is null)
        {
            await Send.NotFoundAsync("Dish not found.", cancellationToken);
            return;
        }

        await Send.OkAsync(row.ToGetByIdResponse(), cancellationToken);
    }
}
