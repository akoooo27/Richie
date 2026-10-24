using System.Security.Cryptography;
using System.Text.Json;

using Microsoft.AspNetCore.DataProtection;

namespace Identity.API.Authentication.Otp;

internal sealed class OtpChallengeCookie(IDataProtectionProvider dataProtectionProvider)
{
    private const string CookieName = "__Host-Richie.OtpChallenge";

    private readonly ITimeLimitedDataProtector _protector = dataProtectionProvider
        .CreateProtector("Identity.API.OtpChallenge.v1")
        .ToTimeLimitedDataProtector();

    public void Write
    (
        HttpResponse response,
        OtpChallenge challenge,
        DateTimeOffset expiresAtUtc
    )
    {
        string payload = _protector.Protect(JsonSerializer.Serialize(challenge), expiresAtUtc);

        response.Cookies.Append(CookieName, payload, CreateOptions(expiresAtUtc));
    }

    public OtpChallenge? Read(HttpRequest request)
    {
        if (!request.Cookies.TryGetValue(CookieName, out string? payload))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<OtpChallenge>(_protector.Unprotect(payload));
        }
        catch (Exception exception) when (exception is CryptographicException or JsonException)
        {
            return null;
        }
    }

    public static void Delete(HttpResponse response) =>
        response.Cookies.Delete(CookieName, CreateOptions(expires: null));

    private static CookieOptions CreateOptions(DateTimeOffset? expires) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        IsEssential = true,
        Path = "/",
        Expires = expires
    };
}
