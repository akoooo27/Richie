using Duende.Storage;

namespace Identity.API.Storage;

internal static class IdentityUsersDatabase
{
    public const string ConnectionStringName = "identity-users-db";

    public static readonly StorageInstanceId InstanceId = StorageInstanceId.Create("identity-users");

    public const string Schema = "identity_users";
}
