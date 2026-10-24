using System.ComponentModel.DataAnnotations;

namespace Identity.API.Pages.Account.Create;

internal sealed class InputModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Full name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
