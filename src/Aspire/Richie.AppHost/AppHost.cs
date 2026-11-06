using Aspire.Hosting.Azure;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<AzureKeyVaultResource> keyVault = builder.AddAzureKeyVault("key-vault");

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> identityUsersDb = postgres.AddDatabase("identity-users-db");

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(keyVault)
    .WithReference(identityDb)
    .WithReference(identityUsersDb)
    .WaitFor(keyVault)
    .WaitFor(identityDb)
    .WaitFor(identityUsersDb);

await builder
    .Build()
    .RunAsync();
