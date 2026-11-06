using Duende.UserManagement;
using Duende.UserManagement.Authentication.Otp;

namespace Identity.API.Authentication.Otp;

internal closed record OtpChallenge(OtpToken Token, EmailAddress Email);

