using Duende.IdentityServer;
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

    public static IEnumerable<Client> GetClients
    (
        string webBffBaseUrl,
        string webBffClientId,
        string webBffClientSecret
    )
    {
        return
        [
            new Client
            {
                ClientId = webBffClientId,
                ClientName = "Web BFF",
                ClientSecrets = { new Secret(webBffClientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = false,
                RedirectUris = { $"{webBffBaseUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{webBffBaseUrl}/signout-callback-oidc" },
                BackChannelLogoutUri = $"{webBffBaseUrl}/bff/backchannel",
                AllowOfflineAccess = true,
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                ]
            }
        ];
    }

}
