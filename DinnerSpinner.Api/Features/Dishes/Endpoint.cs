using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes
{
    internal sealed class Endpoint : Endpoint<Request, Response, Mapper>
    {
        public override void Configure()
        {
            Post("/api/dishes/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await Send.OkAsync(new()
            {
                Name = request.Name,
                Category = request.Category
            }, cancellationToken);
        }
    }
}