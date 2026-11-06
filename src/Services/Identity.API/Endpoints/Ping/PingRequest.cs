namespace Identity.API.Endpoints.Ping;

internal sealed class PingRequest
{
    public string Message { get; init; } = string.Empty;
}
