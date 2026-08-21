using System.ComponentModel.DataAnnotations;

namespace Web.BFF.Configuration;

internal sealed class OidcSettings
{
    public const string SectionName = "Oidc";

    [Required]
    public string Authority { get; init; } = string.Empty;

    [Required]
    public string ClientId { get; init; } = string.Empty;

    [Required]
    public string ClientSecret { get; init; } = string.Empty;
}
