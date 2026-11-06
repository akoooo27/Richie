using Duende.UserManagement;
using Duende.UserManagement.Authentication.Otp;

namespace Identity.API.Authentication.Otp;

internal sealed record OtpSignUpChallenge
(
    string FirstName,
    string LastName,
    OtpToken Token,
    EmailAddress Email
) : OtpChallenge(Token, Email);
