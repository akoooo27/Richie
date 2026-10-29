using Duende.Storage;
using Duende.Storage.Schema;

namespace Identity.API.Storage;

internal static class StorageMigrations
{
    public static async Task MigrateStorageAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();

        IStorageInstanceSchemaFactory schemaFactory =
            scope.ServiceProvider.GetRequiredService<IStorageInstanceSchemaFactory>();

        foreach (StorageInstanceId instanceId in StorageInstances.All)
        {
            IStorageInstanceSchema schema = await schemaFactory.GetStorageInstanceSchema(instanceId, cancellationToken).ConfigureAwait(false);

            await schema.MigrateAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
