using System.Threading;
using System.Threading.Tasks;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Errors;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Read.GetById;

public sealed class Endpoint(AppDbContext db)
    : Endpoint<Request, ApiResponse<Response>>
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
            ThrowError(
                message: "Category not found.",
                errorCode: ErrorCode.NotFound.ToString(),
                statusCode: StatusCodes.Status404NotFound);
        }

        await Send.OkAsync(category!.ToGetByIdResponse(), cancellationToken);
    }
}
