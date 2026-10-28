using Duende.Storage;

namespace Identity.API.Storage;

internal static class StorageInstances
{
    public const string IdentityConnectionName = "identity-db";

    public const string UserManagementConnectionName = "user-management-db";

    public const string SchemaName = "identity";

    public static readonly StorageInstanceId UserManagement = StorageInstanceId.Create("user_management");

    public static readonly IReadOnlyList<StorageInstanceId> All = [StorageInstanceId.Default, UserManagement];
}
