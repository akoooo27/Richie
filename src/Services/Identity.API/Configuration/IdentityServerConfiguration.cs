using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

internal static class IdentityServerConfiguration
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return [];
    }

    public static IEnumerable<Client> GetClients()
    {
        return [];
    }
}
