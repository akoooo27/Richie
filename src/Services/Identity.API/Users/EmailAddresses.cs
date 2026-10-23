using System.Diagnostics.CodeAnalysis;

using Duende.UserManagement;

namespace Identity.API.Users;

internal static class EmailAddresses
{
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase",
        Justification = "Email addresses are stored lowercase.")]
    public static bool TryNormalize(string? input, [NotNullWhen(true)] out EmailAddress? email) =>
        EmailAddress.TryCreate(input?.Trim().ToLowerInvariant(), out email);
}
