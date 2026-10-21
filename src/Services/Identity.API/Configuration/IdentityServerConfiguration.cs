using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

internal static class IdentityServerConfiguration
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
}
