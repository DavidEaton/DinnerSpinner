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
            .Where(dish => dish.Id == request.Id)
            .Select(dish => new DishReadRow(
                dish.Id,
                dish.Name.Value,
                dish.Category.Id,
                dish.Category.Name.Value))
            .SingleOrDefaultAsync(cancellationToken);

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