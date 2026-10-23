using System.ComponentModel.DataAnnotations;

namespace Identity.API.Authentication.Otp;

internal sealed class ResendOtpDispatcherOptions
{
    public const string SectionName = "Authentication:Otp:Resend";

    [Required]
    public string From { get; init; } = string.Empty;

    [Required]
    public string TemplateId { get; init; } = string.Empty;
}
