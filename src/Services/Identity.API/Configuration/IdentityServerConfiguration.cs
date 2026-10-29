using Duende.IdentityServer;
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

    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        string webBffBaseUrl = configuration["Clients:WebBff:BaseUrl"] ??
                               throw new InvalidOperationException("Web BFF base URL is not configured.");

        string webBffClientId = configuration["Clients:WebBff:ClientId"] ??
                                throw new InvalidOperationException("Web BFF client ID is not configured.");

        string webBffClientSecret = configuration["Clients:WebBff:ClientSecret"] ??
                                    throw new InvalidOperationException("Web BFF client secret is not configured.");

        return
        [
            new Client
            {
                ClientId = webBffClientId,
                ClientSecrets = { new Secret(webBffClientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { $"{webBffBaseUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{webBffBaseUrl}/signout-callback-oidc" },
                BackChannelLogoutUri = $"{webBffBaseUrl}/bff/backchannel",

                AllowOfflineAccess = true,
                CoordinateLifetimeWithUserSession = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        ];
    }
}
