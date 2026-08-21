using System.Globalization;

using EFCore.NamingConventions.Internal;

using Identity.API.Database.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Identity.API.Database;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schemas.Identity);

        ApplySnakeCaseToExplicitlyNamedObjects(builder);
    }

    private static void ApplySnakeCaseToExplicitlyNamedObjects(ModelBuilder builder)
    {
        SnakeCaseNameRewriter rewriter = new(CultureInfo.InvariantCulture);

        foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            string? tableName = entityType.GetTableName();

            if (tableName is not null)
            {
                entityType.SetTableName(rewriter.RewriteName(tableName));
            }

            foreach (IMutableIndex index in entityType.GetIndexes())
            {
                string? indexName = index.GetDatabaseName();

                if (indexName is not null)
                {
                    index.SetDatabaseName(rewriter.RewriteName(indexName));
                }
            }
        }
    }
}
