using System.ComponentModel.DataAnnotations;

namespace Identity.API.Configuration;

internal sealed class BffClientSettings
{
    public const string SectionName = "BffClient";

    [Required]
    public string BaseUrl { get; init; } = string.Empty;

    [Required]
    public string Secret { get; init; } = string.Empty;
}
