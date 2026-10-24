using System.ComponentModel.DataAnnotations;

namespace Identity.API.Pages.Account.Verify;

internal sealed class InputModel
{
    [Required]
    [Display(Name = "Verification code")]
    public string Code { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
