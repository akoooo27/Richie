namespace Identity.API.Database;

internal static class Schemas
{
    public const string Operational = "operational";
}

/// <summary>
/// snake_case table names for the Duende operational store (<c>PersistedGrantDbContext</c>).
/// </summary>
internal static class OperationalTables
{
    public const string PersistedGrants = "persisted_grants";
    public const string DeviceCodes = "device_codes";
    public const string Keys = "keys";
    public const string ServerSideSessions = "server_side_sessions";
    public const string PushedAuthorizationRequests = "pushed_authorization_requests";
    public const string SamlSigninStates = "saml_signin_states";
    public const string SamlLogoutSessions = "saml_logout_sessions";
    public const string SamlLogoutSessionRequestIndices = "saml_logout_session_request_indices";
}
