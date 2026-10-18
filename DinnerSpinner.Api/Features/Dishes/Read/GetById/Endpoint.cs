using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Shared;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        if (request.Id <= 0)
        {
            AddError(
                property: request => request.Id,
                errorMessage: "GetBy Id must be a positive integer.",
                severity: Severity.Error,
                errorCode: ErrorCode.Validation.ToString());

            // emits 400 ProblemDetails (our DinnerSpinnerProblemDetails DTO)
            ThrowIfAnyErrors(StatusCodes.Status400BadRequest);
        }

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
            .SingleOrDefaultAsync(row => row.Id == request.Id, cancellationToken);

        if (row is null)
        {
                AddError(
                    property: request => request,
                    errorMessage: "Dish not found.",
                    severity: Severity.Error,
                    errorCode: ErrorCode.NotFound.ToString());

                ThrowIfAnyErrors(StatusCodes.Status404NotFound);
        }

        await Send.OkAsync(row!.ToGetByIdResponse(), cancellationToken);
    }
}