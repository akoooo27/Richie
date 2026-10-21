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

    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        string bffBaseUrl = configuration["Clients:WebBff:BaseUrl"]!.TrimEnd('/');

        return
        [
            new Client
            {
                ClientId = "web-bff",
                ClientName = "Richie Web",
                ClientSecrets = { new Secret(configuration["Clients:WebBff:Secret"]!.Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { $"{bffBaseUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{bffBaseUrl}/signout-callback-oidc" },

                BackChannelLogoutUri = $"{bffBaseUrl}/bff/backchannel",

                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                RequireConsent = false
            }
        ];
    }
}
