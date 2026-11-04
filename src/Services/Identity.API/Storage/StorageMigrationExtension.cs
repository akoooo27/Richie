using Duende.Storage;
using Duende.Storage.Schema;

namespace Identity.API.Storage;


internal static class StorageMigrationExtensions
{
    public static async Task MigrateStorageAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        IStorageInstanceSchemaFactory factory = app.Services.GetRequiredService<IStorageInstanceSchemaFactory>();

        StorageInstanceId[] instances =
        [
            StorageInstanceId.Default,
            IdentityUsersDatabase.InstanceId,
        ];

        foreach (StorageInstanceId instanceId in instances)
        {
            IStorageInstanceSchema schema = await factory.GetStorageInstanceSchema(instanceId, cancellationToken);

            await schema.MigrateAsync(cancellationToken);
        }
    }
}
