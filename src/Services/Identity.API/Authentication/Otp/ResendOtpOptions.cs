using System.ComponentModel.DataAnnotations;

namespace Identity.API.Authentication.Otp;

internal sealed class ResendOtpOptions
{
    public const string SectionName = "Authentication:Otp:Resend";

    [Required]
    public string From { get; set; } = string.Empty;

    [Required]
    public string TemplateId { get; set; } = string.Empty;
}
