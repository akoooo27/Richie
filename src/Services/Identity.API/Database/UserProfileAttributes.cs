using Duende.Storage.EntityAttributeValue;
using Duende.UserManagement.Profiles;

namespace Identity.API.Database;

internal static class UserProfileAttributes
{
    public static readonly AttributeDefinition Email = OidcStandardAttributes.Email with { IsUnique = true };

    public static readonly AttributeDefinition Name = OidcStandardAttributes.Name;

    public static IReadOnlyList<AttributeDefinition> All { get; } = [Email, Name];
}
