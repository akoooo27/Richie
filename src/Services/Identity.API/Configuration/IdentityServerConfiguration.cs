using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

internal static class IdentityServerConfiguration
{
    public const string WebBffClientId = "web-bff";

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    public static IEnumerable<Client> GetClients(BffClientSettings bffClient)
    {
        string baseUrl = bffClient.BaseUrl.TrimEnd('/');

        return new List<Client>
        {
            new()
            {
                ClientId = WebBffClientId,
                ClientSecrets = { new Secret(bffClient.Secret.Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { $"{baseUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{baseUrl}/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
}
