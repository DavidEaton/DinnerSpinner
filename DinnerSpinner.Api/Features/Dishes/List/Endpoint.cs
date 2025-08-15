using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes.List
{
    public sealed class Endpoint(IDishService service)
        : EndpointWithoutRequest<List<Response>>
    {
        public override void Configure()
        {
            Get("/api/dishes");
            Summary(summary => summary.Summary = "List dishes");
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var dishes = await service.ListAsync(cancellationToken);
            await Send.OkAsync(dishes.ToList(), cancellationToken);
        }
    }
}
