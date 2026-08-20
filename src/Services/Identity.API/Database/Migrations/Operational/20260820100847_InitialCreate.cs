using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Identity.API.Database.Migrations.Operational
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "operational");

            migrationBuilder.CreateTable(
                name: "device_flow_codes",
                schema: "operational",
                columns: table => new
                {
                    user_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    device_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device_flow_codes", x => x.user_code);
                });

            migrationBuilder.CreateTable(
                name: "keys",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    use = table.Column<string>(type: "text", nullable: true),
                    algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_x509certificate = table.Column<bool>(type: "boolean", nullable: false),
                    data_protected = table.Column<bool>(type: "boolean", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "par",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reference_value_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    parameters = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_par", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "persisted_grants",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consumed_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persisted_grants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "saml_logout_sessions",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    logout_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    serialized_session = table.Column<string>(type: "text", nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_saml_logout_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "saml_signin_states",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state_id = table.Column<Guid>(type: "uuid", nullable: false),
                    serialized_state = table.Column<string>(type: "text", nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    service_provider_entity_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_saml_signin_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "server_side_sessions",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    scheme = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    renewed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_side_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "saml_logout_session_request_indices",
                schema: "operational",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    saml_logout_session_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_saml_logout_session_request_indices", x => x.id);
                    table.ForeignKey(
                        name: "fk_saml_logout_session_request_indices_saml_logout_sessions_sa",
                        column: x => x.saml_logout_session_id,
                        principalSchema: "operational",
                        principalTable: "saml_logout_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_device_flow_codes_device_code",
                schema: "operational",
                table: "device_flow_codes",
                column: "device_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_flow_codes_expiration",
                schema: "operational",
                table: "device_flow_codes",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_keys_use",
                schema: "operational",
                table: "keys",
                column: "use");

            migrationBuilder.CreateIndex(
                name: "ix_par_expires_at_utc",
                schema: "operational",
                table: "par",
                column: "expires_at_utc");

            migrationBuilder.CreateIndex(
                name: "ix_par_reference_value_hash",
                schema: "operational",
                table: "par",
                column: "reference_value_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_consumed_time",
                schema: "operational",
                table: "persisted_grants",
                column: "consumed_time");

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_expiration",
                schema: "operational",
                table: "persisted_grants",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_key",
                schema: "operational",
                table: "persisted_grants",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_subject_id_client_id_type",
                schema: "operational",
                table: "persisted_grants",
                columns: new[] { "subject_id", "client_id", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_subject_id_session_id_type",
                schema: "operational",
                table: "persisted_grants",
                columns: new[] { "subject_id", "session_id", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_saml_logout_session_request_indices_request_id",
                schema: "operational",
                table: "saml_logout_session_request_indices",
                column: "request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_saml_logout_session_request_indices_saml_logout_session_id",
                schema: "operational",
                table: "saml_logout_session_request_indices",
                column: "saml_logout_session_id");

            migrationBuilder.CreateIndex(
                name: "ix_saml_logout_sessions_expires_at_utc",
                schema: "operational",
                table: "saml_logout_sessions",
                column: "expires_at_utc");

            migrationBuilder.CreateIndex(
                name: "ix_saml_logout_sessions_logout_id",
                schema: "operational",
                table: "saml_logout_sessions",
                column: "logout_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_saml_signin_states_expires_at_utc",
                schema: "operational",
                table: "saml_signin_states",
                column: "expires_at_utc");

            migrationBuilder.CreateIndex(
                name: "ix_saml_signin_states_state_id",
                schema: "operational",
                table: "saml_signin_states",
                column: "state_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_server_side_sessions_display_name",
                schema: "operational",
                table: "server_side_sessions",
                column: "display_name");

            migrationBuilder.CreateIndex(
                name: "ix_server_side_sessions_expires",
                schema: "operational",
                table: "server_side_sessions",
                column: "expires");

            migrationBuilder.CreateIndex(
                name: "ix_server_side_sessions_key",
                schema: "operational",
                table: "server_side_sessions",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_server_side_sessions_session_id",
                schema: "operational",
                table: "server_side_sessions",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_side_sessions_subject_id",
                schema: "operational",
                table: "server_side_sessions",
                column: "subject_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_flow_codes",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "keys",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "par",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "persisted_grants",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "saml_logout_session_request_indices",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "saml_signin_states",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "server_side_sessions",
                schema: "operational");

            migrationBuilder.DropTable(
                name: "saml_logout_sessions",
                schema: "operational");
        }
    }
}
