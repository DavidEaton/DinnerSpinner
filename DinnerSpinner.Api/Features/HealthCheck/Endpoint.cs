using FastEndpoints;

namespace DinnerSpinner.Api.Features.HealthCheck
{
    internal class Endpoint : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            Get("/api/health");
            AllowAnonymous();
            Summary(s => s.Summary = "Health check endpoint");
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            await Send.OkAsync(new Response(), cancellationToken);
        }
    }

    public class Response
    {
        public string Status { get; set; } = "Healthy";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
