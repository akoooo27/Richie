namespace Identity.API.Endpoints.Ping;

internal sealed class PingResponse
{
    public required string Message { get; init; }

    public required DateTimeOffset ServerTime { get; init; }
}
