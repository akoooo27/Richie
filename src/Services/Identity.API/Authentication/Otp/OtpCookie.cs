using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;

using Duende.UserManagement;
using Duende.UserManagement.Authentication.Otp;

using Microsoft.AspNetCore.DataProtection;

namespace Identity.API.Authentication.Otp;

internal sealed class OtpCookie(IDataProtectionProvider dataProtectionProvider)
{
    private const string CookieName = "__Host-OtpChallenge";

    private readonly ITimeLimitedDataProtector _protector = dataProtectionProvider
        .CreateProtector("Identity.API.OtpChallenge.v1")
        .ToTimeLimitedDataProtector();

    public void Write(HttpContext httpContext, OtpChallenge challenge, DateTimeOffset expiresAtUtc)
    {
        Payload payload = ToPayload(challenge);

        string json = JsonSerializer.Serialize(payload);

        string protectedValue = _protector.Protect(json, expiresAtUtc);

        httpContext.Response.Cookies.Append
        (
            key: CookieName,
            value: protectedValue,
            options: BuildOptions(expiresAtUtc)
        );
    }

    public bool TryRead(HttpContext httpContext, [NotNullWhen(true)] out OtpChallenge? challenge)
    {
        challenge = null;

        if (!httpContext.Request.Cookies.TryGetValue(CookieName, out string? protectedValue))
        {
            return false;
        }

        Payload? payload;

        try
        {
            payload = JsonSerializer.Deserialize<Payload>(_protector.Unprotect(protectedValue));
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }

        if (payload is null || !OtpToken.TryCreate(payload.Token, out OtpToken? token) ||
            !EmailAddress.TryCreate(payload.Email, out EmailAddress? email))
        {
            return false;
        }

        challenge = payload.Kind switch
        {
            Kind.SignIn => new OtpSignInChallenge(token, email),
            Kind.SignUp when payload is { FirstName: { } firstName, LastName: { } lastName } =>
                new OtpSignUpChallenge
                (
                    FirstName: firstName,
                    LastName: lastName,
                    Token: token,
                    Email: email
                ),
            _ => null
        };

        return challenge is not null;
    }

    public static void Clear(HttpContext httpContext) =>
        httpContext.Response.Cookies.Delete(CookieName, BuildOptions());

    private static Payload ToPayload(OtpChallenge challenge) => challenge switch
    {
        OtpSignInChallenge signInChallenge => new Payload
        (
            Kind: Kind.SignIn,
            Token: signInChallenge.Token.ToString(),
            Email: signInChallenge.Email.Value,
            FirstName: null,
            LastName: null
        ),
        OtpSignUpChallenge signUpChallenge => new Payload
        (
            Kind: Kind.SignUp,
            Token: signUpChallenge.Token.ToString(),
            Email: signUpChallenge.Email.Value,
            FirstName: signUpChallenge.FirstName,
            LastName: signUpChallenge.LastName
        )
    };

    private static CookieOptions BuildOptions(DateTimeOffset? expiresAtUtc = null) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        IsEssential = true,
        Path = "/",
        Expires = expiresAtUtc,
    };

    private enum Kind
    {
        SignIn = 1,
        SignUp = 2,
    }

    private sealed record Payload
    (
        Kind Kind,
        string Token,
        string Email,
        string? FirstName,
        string? LastName
    );
}
