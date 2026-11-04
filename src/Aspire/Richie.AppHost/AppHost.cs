IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identity-db");

IResourceBuilder<PostgresDatabaseResource> identityUsersDb = postgres.AddDatabase("identity-users-db");

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb)
    .WithReference(identityUsersDb)
    .WaitFor(identityDb)
    .WaitFor(identityUsersDb);

await builder
    .Build()
    .RunAsync();
