using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Features.Categories.Create
{
    public class Endpoint(AppDbContext db)
        : Endpoint<Request, ApiResponse<Response>>
    {
        public override void Configure()
        {
            Post("/api/categories");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new category");
        }

        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exists = await db.Categories.AnyAsync(c => c.Name == name, cancellationToken);
            if (exists)
            {
                await Send.ErrorAsync(ApiError.Conflict("Category already exists."), cancellationToken);
                return;
            }

            var category = new Category { Name = name };
            db.Categories.Add(category);
            await db.SaveChangesAsync(cancellationToken);
            var createResponse = category.ToCreateResponse();

            await Send.ResponseAsync(ApiResponse<Response>.Ok(createResponse), 201, cancellationToken);
        }
    }
}
