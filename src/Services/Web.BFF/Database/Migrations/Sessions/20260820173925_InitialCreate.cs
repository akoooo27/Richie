using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.BFF.Database.Migrations.Sessions
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sessions");

            migrationBuilder.CreateTable(
                name: "user_sessions",
                schema: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    session_id = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    renewed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ticket = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    partition_key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_sessions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_expires",
                schema: "sessions",
                table: "user_sessions",
                column: "expires");

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_partition_key_key",
                schema: "sessions",
                table: "user_sessions",
                columns: new[] { "partition_key", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_partition_key_session_id",
                schema: "sessions",
                table: "user_sessions",
                columns: new[] { "partition_key", "session_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_partition_key_subject_id_session_id",
                schema: "sessions",
                table: "user_sessions",
                columns: new[] { "partition_key", "subject_id", "session_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_sessions",
                schema: "sessions");
        }
    }
}
