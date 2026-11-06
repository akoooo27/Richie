using FastEndpoints;

namespace Identity.API.Endpoints.Ping;

internal sealed class PingEndpoint : Endpoint<PingRequest, PingResponse>
{
    public override void Configure()
    {
        Get("/ping");
        AllowAnonymous();
        Summary(s => s.Summary = "Echoes a message back with the server time.");
    }

    public override async Task HandleAsync(PingRequest req, CancellationToken ct)
    {
        await Send.OkAsync(new PingResponse
        {
            Message = req.Message,
            ServerTime = DateTimeOffset.UtcNow,
        }, ct);
    }
}
