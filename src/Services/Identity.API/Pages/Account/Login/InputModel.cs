using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Pages.Account.Login;

internal sealed class InputModel
{
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
