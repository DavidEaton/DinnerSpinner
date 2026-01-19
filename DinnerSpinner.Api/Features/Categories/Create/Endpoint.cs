using CSharpFunctionalExtensions;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using static DinnerSpinner.Api.Common.Errors;

namespace DinnerSpinner.Api.Features.Categories.Create
{
    public class Endpoint(AppDbContext db)
        : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/api/categories/create");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new category");
        }
        public override async Task<Result<Category, Error>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = new Category
            {
                Name = request.Name,
            };

            db.Categories.Add(entity);
            await db.SaveChangesAsync(cancellationToken);

            await Send.CreatedAtAsync<Read.GetById.Endpoint>(
                routeValues: new { id = entity.Id },
                responseBody: entity.ToCreateResponse(),
                cancellation: cancellationToken);

            return Result.Success<Category, Error>(entity);
        }
    }
}
