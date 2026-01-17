using FastEndpoints;

namespace DinnerSpinner.Api.Features.HealthCheck
{
    internal class Endpoint : EndpointWithoutRequest<HealthCheckResponse>
    {
        public override void Configure()
        {
            Get("/api/health");
            AllowAnonymous();
            Summary(s => s.Summary = "Health check endpoint");
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            await Send.OkAsync(new HealthCheckResponse(), cancellationToken);
        }
    }

    public class HealthCheckResponse
    {
        public string Status { get; set; } = "Healthy";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
