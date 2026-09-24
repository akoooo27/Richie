using Identity.API.Database.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Database;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schemas.Identity);

        builder.Entity<ApplicationUser>(users =>
        {
            users
                .ToTable("users");

            users
                .HasIndex(user => user.NormalizedUserName)
                .HasDatabaseName("ix_users_normalized_user_name");

            users
                .HasIndex(user => user.NormalizedEmail)
                .HasDatabaseName("ix_users_normalized_email");
        });

        builder.Entity<ApplicationRole>(roles =>
        {
            roles
                .ToTable("roles");

            roles
                .HasIndex(role => role.NormalizedName)
                .HasDatabaseName("ix_roles_normalized_name");
        });

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        if (builder.Model.FindEntityType(typeof(IdentityUserPasskey<Guid>)) is not null)
        {
            builder.Entity<IdentityUserPasskey<Guid>>()
                .ToTable("user_passkeys");
        }
    }
}
