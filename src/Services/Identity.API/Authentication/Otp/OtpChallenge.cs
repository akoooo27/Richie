namespace Identity.API.Authentication.Otp;

internal sealed record OtpChallenge
(
    Guid Token,
    string Email,
    string? Name
);
