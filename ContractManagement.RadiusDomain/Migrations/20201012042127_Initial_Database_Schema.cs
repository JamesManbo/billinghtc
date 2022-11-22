using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.RadiusDomain.Migrations
{
    public partial class Initial_Database_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nasname = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    shortname = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    type = table.Column<string>(type: "varchar(30)", nullable: true, defaultValueSql: "'other'")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ports = table.Column<int>(type: "int(5)", nullable: true),
                    secret = table.Column<string>(type: "varchar(60)", nullable: false, defaultValueSql: "'secret'")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    community = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    description = table.Column<string>(type: "varchar(200)", nullable: true, defaultValueSql: "'RADIUS Client'")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    starospassword = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ciscobwmode = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    apiusername = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    apipassword = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    enableapi = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radacct",
                columns: table => new
                {
                    radacctid = table.Column<long>(type: "bigint(21)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    acctsessionid = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctuniqueid = table.Column<string>(type: "varchar(32)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    groupname = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    realm = table.Column<string>(type: "varchar(64)", nullable: true, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    nasipaddress = table.Column<string>(type: "varchar(15)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    nasportid = table.Column<string>(type: "varchar(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    nasporttype = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctstarttime = table.Column<DateTime>(nullable: true),
                    acctstoptime = table.Column<DateTime>(nullable: true),
                    acctsessiontime = table.Column<int>(type: "int(12)", nullable: true),
                    acctauthentic = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    connectinfo_start = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    connectinfo_stop = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctinputoctets = table.Column<long>(type: "bigint(20)", nullable: true),
                    acctoutputoctets = table.Column<long>(type: "bigint(20)", nullable: true),
                    calledstationid = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    callingstationid = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctterminatecause = table.Column<string>(type: "varchar(32)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    servicetype = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    framedprotocol = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    framedipaddress = table.Column<string>(type: "varchar(15)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctstartdelay = table.Column<int>(type: "int(12)", nullable: true),
                    acctstopdelay = table.Column<int>(type: "int(12)", nullable: true),
                    xascendsessionsvrkey = table.Column<string>(type: "varchar(10)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    _accttime = table.Column<DateTime>(nullable: true),
                    _srvid = table.Column<int>(type: "int(11)", nullable: true),
                    _dailynextsrvactive = table.Column<sbyte>(type: "tinyint(1)", nullable: true),
                    _apid = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radacct", x => x.radacctid);
                });

            migrationBuilder.CreateTable(
                name: "radcheck",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int(11) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    attribute = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    op = table.Column<string>(type: "char(2)", nullable: false, defaultValueSql: "'=='")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    value = table.Column<string>(type: "varchar(253)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radcheck", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radgroupcheck",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int(11) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    groupname = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    attribute = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    op = table.Column<string>(type: "char(2)", nullable: false, defaultValueSql: "'=='")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    value = table.Column<string>(type: "varchar(253)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupcheck", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radgroupreply",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int(11) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    groupname = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    attribute = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    op = table.Column<string>(type: "char(2)", nullable: false, defaultValueSql: "'='")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    value = table.Column<string>(type: "varchar(253)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupreply", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radippool",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int(11) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pool_name = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    framedipaddress = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    nasipaddress = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    calledstationid = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    callingstationid = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    expiry_time = table.Column<DateTime>(nullable: true),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    pool_key = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radippool", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radpostauth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    pass = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    reply = table.Column<string>(type: "varchar(32)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    authdate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    nasipaddress = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radpostauth", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radreply",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int(11) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    attribute = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    op = table.Column<string>(type: "char(2)", nullable: false, defaultValueSql: "'='")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    value = table.Column<string>(type: "varchar(253)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radreply", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radusergroup",
                columns: table => new
                {
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    groupname = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    priority = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_actsrv",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(nullable: false),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    dailynextsrvactive = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_actsrv", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_allowedmanagers",
                columns: table => new
                {
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_allowednases",
                columns: table => new
                {
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    nasid = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_ap",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    enable = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    accessmode = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    community = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    apiusername = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    apipassword = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    description = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_ap", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_cards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cardnum = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    password = table.Column<string>(type: "varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    value = table.Column<decimal>(type: "decimal(22,2)", nullable: false),
                    expiration = table.Column<DateTime>(type: "date", nullable: false),
                    series = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    owner = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    used = table.Column<DateTime>(nullable: false),
                    cardtype = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    revoked = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    downlimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    uplimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    comblimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    uptimelimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    transid = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    active = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    expiretime = table.Column<long>(type: "bigint(20)", nullable: false),
                    timebaseexp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timebaseonline = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_changesrv",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    newsrvid = table.Column<int>(type: "int(11)", nullable: false),
                    newsrvname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    scheduledate = table.Column<DateTime>(type: "date", nullable: false),
                    requestdate = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    transid = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    requested = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_changesrv", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_cmts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    community = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    descr = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_cmts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_colsetlistdocsis",
                columns: table => new
                {
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    colname = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_colsetlistradius",
                columns: table => new
                {
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    colname = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_colsetlistusers",
                columns: table => new
                {
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    colname = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_ias",
                columns: table => new
                {
                    iasid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iasname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    price = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    downlimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    uplimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    comblimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    uptimelimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    expiretime = table.Column<long>(type: "bigint(20)", nullable: false),
                    timebaseonline = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timebaseexp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    enableias = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    expiremode = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    expiration = table.Column<DateTime>(type: "date", nullable: false),
                    simuse = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.iasid);
                });

            migrationBuilder.CreateTable(
                name: "rm_invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    invgroup = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    invnum = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    bytesdl = table.Column<long>(type: "bigint(20)", nullable: false),
                    bytesul = table.Column<long>(type: "bigint(20)", nullable: false),
                    bytescomb = table.Column<long>(type: "bigint(20)", nullable: false),
                    downlimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    uplimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    comblimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    time = table.Column<int>(type: "int(11)", nullable: false),
                    uptimelimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    days = table.Column<int>(type: "int(6)", nullable: false),
                    expiration = table.Column<DateTime>(type: "date", nullable: false),
                    capdl = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    capul = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    captotal = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    captime = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    capdate = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    service = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    comment = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    transid = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    amount = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    address = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    city = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    zip = table.Column<string>(type: "varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    country = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    state = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    fullname = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    taxid = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    paymentopt = table.Column<DateTime>(type: "date", nullable: false),
                    invtype = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    paymode = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    paid = table.Column<DateTime>(type: "date", nullable: false),
                    price = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    tax = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    vatpercent = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    remark = table.Column<string>(type: "varchar(400)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    balance = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    gwtransid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    mobile = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_invoices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_ippools",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    fromip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    toip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    descr = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    nextpoolid = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_ippools", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_managers",
                columns: table => new
                {
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    password = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    firstname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lastname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    mobile = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    address = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    city = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    zip = table.Column<string>(type: "varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    country = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    state = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    comment = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    company = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    vatid = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    email = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    balance = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    perm_listusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_createusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_editusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_edituserspriv = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_deleteusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_listmanagers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_createmanagers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_editmanagers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_deletemanagers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_listservices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_createservices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_editservices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_deleteservices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_listonlineusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_listinvoices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_trafficreport = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_addcredits = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_negbalance = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_listallinvoices = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_showinvtotals = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_logout = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_cardsys = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_editinvoice = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_allusers = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_allowdiscount = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_enwriteoff = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_accessap = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    perm_cts = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    enablemanager = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    lang = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.managername);
                });

            migrationBuilder.CreateTable(
                name: "rm_newusers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    firstname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lastname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    address = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    city = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    zip = table.Column<string>(type: "varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    country = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    state = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    mobile = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    email = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    vatid = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    actcode = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    actcount = table.Column<int>(type: "int(11)", nullable: false),
                    lang = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_newusers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_onlinecm",
                columns: table => new
                {
                    username = table.Column<string>(type: "varchar(64)", nullable: false, defaultValueSql: "''")
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    maccm = table.Column<string>(type: "varchar(17)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    enableuser = table.Column<sbyte>(type: "tinyint(1)", nullable: true),
                    staticipcm = table.Column<string>(type: "varchar(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    maccpe = table.Column<string>(type: "varchar(17)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ipcpe = table.Column<string>(type: "varchar(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ipmodecpe = table.Column<sbyte>(type: "tinyint(1)", nullable: true),
                    cmtsid = table.Column<int>(type: "int(11)", nullable: true),
                    groupid = table.Column<int>(type: "int(11)", nullable: true),
                    groupname = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    snrds = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    snrus = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    txpwr = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    rxpwr = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    pingtime = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    upstreamname = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ifidx = table.Column<int>(type: "int(11)", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "rm_phpsess",
                columns: table => new
                {
                    managername = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    sessid = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lastact = table.Column<DateTime>(nullable: false),
                    closed = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_radacct",
                columns: table => new
                {
                    radacctid = table.Column<long>(type: "bigint(21)", nullable: false),
                    acctuniqueid = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    acctstarttime = table.Column<DateTime>(nullable: false),
                    acctstoptime = table.Column<DateTime>(nullable: false),
                    acctsessiontime = table.Column<int>(type: "int(12)", nullable: false),
                    acctsessiontimeratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    dlbytesstart = table.Column<long>(type: "bigint(20)", nullable: false),
                    dlbytesstop = table.Column<long>(type: "bigint(20)", nullable: false),
                    dlbytes = table.Column<long>(type: "bigint(20)", nullable: false),
                    dlratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ulbytesstart = table.Column<long>(type: "bigint(20)", nullable: false),
                    ulbytesstop = table.Column<long>(type: "bigint(20)", nullable: false),
                    ulbytes = table.Column<long>(type: "bigint(20)", nullable: false),
                    ulratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_services",
                columns: table => new
                {
                    srvid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    srvname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    descr = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    downrate = table.Column<int>(type: "int(11)", nullable: false),
                    uprate = table.Column<int>(type: "int(11)", nullable: false),
                    limitdl = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    limitul = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    limitcomb = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    limitexpiration = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    limituptime = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    poolname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    unitprice = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    unitpriceadd = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    timebaseexp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timebaseonline = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timeunitexp = table.Column<int>(type: "int(11)", nullable: false),
                    timeunitonline = table.Column<int>(type: "int(11)", nullable: false),
                    trafficunitdl = table.Column<int>(type: "int(11)", nullable: false),
                    trafficunitul = table.Column<int>(type: "int(11)", nullable: false),
                    trafficunitcomb = table.Column<int>(type: "int(11)", nullable: false),
                    inittimeexp = table.Column<int>(type: "int(11)", nullable: false),
                    inittimeonline = table.Column<int>(type: "int(11)", nullable: false),
                    initdl = table.Column<int>(type: "int(11)", nullable: false),
                    initul = table.Column<int>(type: "int(11)", nullable: false),
                    inittotal = table.Column<int>(type: "int(11)", nullable: false),
                    srvtype = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timeaddmodeexp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    timeaddmodeonline = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    trafficaddmode = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    monthly = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    enaddcredits = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    minamount = table.Column<int>(type: "int(20)", nullable: false),
                    minamountadd = table.Column<int>(type: "int(20)", nullable: false),
                    resetcounters = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pricecalcdownload = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pricecalcupload = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pricecalcuptime = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    unitpricetax = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    unitpriceaddtax = table.Column<decimal>(type: "decimal(25,6)", nullable: false),
                    enableburst = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    dlburstlimit = table.Column<int>(type: "int(11)", nullable: false),
                    ulburstlimit = table.Column<int>(type: "int(11)", nullable: false),
                    dlburstthreshold = table.Column<int>(type: "int(11)", nullable: false),
                    ulburstthreshold = table.Column<int>(type: "int(11)", nullable: false),
                    dlbursttime = table.Column<int>(type: "int(11)", nullable: false),
                    ulbursttime = table.Column<int>(type: "int(11)", nullable: false),
                    enableservice = table.Column<int>(type: "int(11)", nullable: false),
                    dlquota = table.Column<long>(type: "bigint(20)", nullable: false),
                    ulquota = table.Column<long>(type: "bigint(20)", nullable: false),
                    combquota = table.Column<long>(type: "bigint(20)", nullable: false),
                    timequota = table.Column<long>(type: "bigint(20)", nullable: false),
                    priority = table.Column<short>(type: "smallint(6)", nullable: false),
                    nextsrvid = table.Column<int>(type: "int(11)", nullable: false),
                    dailynextsrvid = table.Column<int>(type: "int(11)", nullable: false),
                    disnextsrvid = table.Column<int>(type: "int(11)", nullable: false),
                    availucp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    renew = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    carryover = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    policymapdl = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    policymapul = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    custattr = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    gentftp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    cmcfg = table.Column<string>(type: "varchar(10240)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    advcmcfg = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    addamount = table.Column<int>(type: "int(11)", nullable: false),
                    ignstatip = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.srvid);
                });

            migrationBuilder.CreateTable(
                name: "rm_settings",
                columns: table => new
                {
                    currency = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    unixacc = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    diskquota = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    quotatpl = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    paymentopt = table.Column<int>(type: "int(11)", nullable: false),
                    changesrv = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    vatpercent = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    disablenotpaid = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    disableexpcont = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    resetctr = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    newnasallsrv = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    newmanallsrv = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    disconnmethod = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    warndl = table.Column<long>(type: "bigint(20)", nullable: false),
                    warndlpercent = table.Column<int>(type: "int(3)", nullable: false),
                    warnul = table.Column<long>(type: "bigint(20)", nullable: false),
                    warnulpercent = table.Column<int>(type: "int(3)", nullable: false),
                    warncomb = table.Column<long>(type: "bigint(20)", nullable: false),
                    warncombpercent = table.Column<int>(type: "int(3)", nullable: false),
                    warnuptime = table.Column<long>(type: "bigint(20)", nullable: false),
                    warnuptimepercent = table.Column<int>(type: "int(3)", nullable: false),
                    warnexpiry = table.Column<int>(type: "int(11)", nullable: false),
                    emailselfregman = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    emailwelcome = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    emailnewsrv = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    emailrenew = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    emailexpiry = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    smswelcome = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    smsexpiry = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    warnmode = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    edituserdata = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    hidelimits = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_internal = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_paypalstd = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_paypalpro = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_paypalexp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_sagepay = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_authorizenet = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_dps = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_2co = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    pm_payfast = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    unixhost = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    remotehostname = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    maclock = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    billingstart = table.Column<sbyte>(type: "tinyint(2)", nullable: false),
                    renewday = table.Column<sbyte>(type: "tinyint(2)", nullable: false),
                    changepswucp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    redeemucp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    buycreditsucp = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_firstname = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_lastname = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_address = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_city = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_zip = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_country = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_state = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_phone = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_mobile = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_email = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_mobactsms = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_nameactemail = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_nameactsms = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_endupemail = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_endupmobile = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg_vatid = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ias_email = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ias_mobile = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ias_verify = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ias_endupemail = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ias_endupmobile = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    simuseselfreg = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "rm_specperacnt",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    starttime = table.Column<TimeSpan>(nullable: false),
                    endtime = table.Column<TimeSpan>(nullable: false),
                    timeratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    dlratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ulratio = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    connallowed = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    mon = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    tue = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    wed = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    thu = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    fri = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    sat = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    sun = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_specperacnt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_specperbw",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    starttime = table.Column<TimeSpan>(nullable: false),
                    endtime = table.Column<TimeSpan>(nullable: false),
                    dlrate = table.Column<int>(type: "int(11)", nullable: false),
                    ulrate = table.Column<int>(type: "int(11)", nullable: false),
                    dlburstlimit = table.Column<int>(type: "int(11)", nullable: false),
                    ulburstlimit = table.Column<int>(type: "int(11)", nullable: false),
                    dlburstthreshold = table.Column<int>(type: "int(11)", nullable: false),
                    ulburstthreshold = table.Column<int>(type: "int(11)", nullable: false),
                    dlbursttime = table.Column<int>(type: "int(11)", nullable: false),
                    ulbursttime = table.Column<int>(type: "int(11)", nullable: false),
                    enableburst = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    priority = table.Column<int>(type: "int(11)", nullable: false),
                    mon = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    tue = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    wed = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    thu = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    fri = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    sat = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    sun = table.Column<sbyte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_specperbw", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_syslog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    datetime = table.Column<DateTime>(nullable: false),
                    ip = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    name = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    eventid = table.Column<int>(type: "int(11)", nullable: false),
                    data1 = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rm_syslog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rm_usergroups",
                columns: table => new
                {
                    groupid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    groupname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    descr = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.groupid);
                });

            migrationBuilder.CreateTable(
                name: "rm_users",
                columns: table => new
                {
                    username = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    password = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    groupid = table.Column<int>(type: "int(11)", nullable: false),
                    enableuser = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    uplimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    downlimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    comblimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    firstname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lastname = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    company = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    phone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    mobile = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    address = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    city = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    zip = table.Column<string>(type: "varchar(8)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    country = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    state = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    comment = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    gpslat = table.Column<decimal>(type: "decimal(17,14)", nullable: false),
                    gpslong = table.Column<decimal>(type: "decimal(17,14)", nullable: false),
                    mac = table.Column<string>(type: "varchar(17)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    usemacauth = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    expiration = table.Column<DateTime>(nullable: false),
                    uptimelimit = table.Column<long>(type: "bigint(20)", nullable: false),
                    srvid = table.Column<int>(type: "int(11)", nullable: false),
                    staticipcm = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    staticipcpe = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ipmodecm = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    ipmodecpe = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    poolidcm = table.Column<int>(type: "int(11)", nullable: false),
                    poolidcpe = table.Column<int>(type: "int(11)", nullable: false),
                    createdon = table.Column<DateTime>(type: "date", nullable: false),
                    acctype = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    credits = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    cardfails = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    createdby = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    owner = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    taxid = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    email = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    maccm = table.Column<string>(type: "varchar(17)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    custattr = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    warningsent = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    verifycode = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    verified = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    selfreg = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    verifyfails = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    verifysentnum = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    verifymobile = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    contractid = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    contractvalid = table.Column<DateTime>(type: "date", nullable: false),
                    actcode = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    pswactsmsnum = table.Column<sbyte>(type: "tinyint(4)", nullable: false),
                    alertemail = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    alertsms = table.Column<sbyte>(type: "tinyint(1)", nullable: false),
                    lang = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lastlogoff = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "rm_wlan",
                columns: table => new
                {
                    maccpe = table.Column<string>(type: "varchar(17)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    signal = table.Column<short>(type: "smallint(6)", nullable: true),
                    ccq = table.Column<short>(type: "smallint(6)", nullable: true),
                    snr = table.Column<short>(type: "smallint(6)", nullable: true),
                    apip = table.Column<string>(type: "varchar(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    timestamp = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "nasname",
                table: "nas",
                column: "nasname");

            migrationBuilder.CreateIndex(
                name: "acctsessionid",
                table: "radacct",
                column: "acctsessionid");

            migrationBuilder.CreateIndex(
                name: "acctsessiontime",
                table: "radacct",
                column: "acctsessiontime");

            migrationBuilder.CreateIndex(
                name: "acctstarttime",
                table: "radacct",
                column: "acctstarttime");

            migrationBuilder.CreateIndex(
                name: "acctstoptime",
                table: "radacct",
                column: "acctstoptime");

            migrationBuilder.CreateIndex(
                name: "_AcctTime",
                table: "radacct",
                column: "_accttime");

            migrationBuilder.CreateIndex(
                name: "acctuniqueid",
                table: "radacct",
                column: "acctuniqueid");

            migrationBuilder.CreateIndex(
                name: "callingstationid",
                table: "radacct",
                column: "callingstationid");

            migrationBuilder.CreateIndex(
                name: "framedipaddress",
                table: "radacct",
                column: "framedipaddress");

            migrationBuilder.CreateIndex(
                name: "nasipaddress",
                table: "radacct",
                column: "nasipaddress");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "radacct",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "radcheck",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "groupname",
                table: "radgroupcheck",
                column: "groupname");

            migrationBuilder.CreateIndex(
                name: "groupname",
                table: "radgroupreply",
                column: "groupname");

            migrationBuilder.CreateIndex(
                name: "authdate",
                table: "radpostauth",
                column: "authdate");

            migrationBuilder.CreateIndex(
                name: "nasipaddress",
                table: "radpostauth",
                column: "nasipaddress");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "radpostauth",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "radreply",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "radusergroup",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "datetime",
                table: "rm_actsrv",
                column: "datetime");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "rm_actsrv",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "username",
                table: "rm_actsrv",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_allowedmanagers",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "srvid",
                table: "rm_allowedmanagers",
                column: "srvid");

            migrationBuilder.CreateIndex(
                name: "nasid",
                table: "rm_allowednases",
                column: "nasid");

            migrationBuilder.CreateIndex(
                name: "srvid",
                table: "rm_allowednases",
                column: "srvid");

            migrationBuilder.CreateIndex(
                name: "ip",
                table: "rm_ap",
                column: "ip");

            migrationBuilder.CreateIndex(
                name: "cardnum",
                table: "rm_cards",
                column: "cardnum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "owner",
                table: "rm_cards",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "series",
                table: "rm_cards",
                column: "series");

            migrationBuilder.CreateIndex(
                name: "used",
                table: "rm_cards",
                column: "used");

            migrationBuilder.CreateIndex(
                name: "requestdate",
                table: "rm_changesrv",
                column: "requestdate");

            migrationBuilder.CreateIndex(
                name: "scheduledate",
                table: "rm_changesrv",
                column: "scheduledate");

            migrationBuilder.CreateIndex(
                name: "ip",
                table: "rm_cmts",
                column: "ip");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_colsetlistdocsis",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_colsetlistradius",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_colsetlistusers",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "comment",
                table: "rm_invoices",
                column: "comment");

            migrationBuilder.CreateIndex(
                name: "date",
                table: "rm_invoices",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "gwtransid",
                table: "rm_invoices",
                column: "gwtransid");

            migrationBuilder.CreateIndex(
                name: "invgroup",
                table: "rm_invoices",
                column: "invgroup");

            migrationBuilder.CreateIndex(
                name: "invnum",
                table: "rm_invoices",
                column: "invnum");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_invoices",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "paid",
                table: "rm_invoices",
                column: "paid");

            migrationBuilder.CreateIndex(
                name: "paymode",
                table: "rm_invoices",
                column: "paymode");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "rm_invoices",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "name",
                table: "rm_ippools",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "nextid",
                table: "rm_ippools",
                column: "nextpoolid");

            migrationBuilder.CreateIndex(
                name: "groupname",
                table: "rm_onlinecm",
                column: "groupname");

            migrationBuilder.CreateIndex(
                name: "ipcpe",
                table: "rm_onlinecm",
                column: "ipcpe");

            migrationBuilder.CreateIndex(
                name: "maccm",
                table: "rm_onlinecm",
                column: "maccm");

            migrationBuilder.CreateIndex(
                name: "staticipcm",
                table: "rm_onlinecm",
                column: "staticipcm");

            migrationBuilder.CreateIndex(
                name: "managername",
                table: "rm_phpsess",
                column: "managername");

            migrationBuilder.CreateIndex(
                name: "acctstarttime",
                table: "rm_radacct",
                column: "acctstarttime");

            migrationBuilder.CreateIndex(
                name: "acctstoptime",
                table: "rm_radacct",
                column: "acctstoptime");

            migrationBuilder.CreateIndex(
                name: "acctuniqueid",
                table: "rm_radacct",
                column: "acctuniqueid");

            migrationBuilder.CreateIndex(
                name: "radacctid",
                table: "rm_radacct",
                column: "radacctid");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "rm_radacct",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "srvname",
                table: "rm_services",
                column: "srvname");

            migrationBuilder.CreateIndex(
                name: "totime",
                table: "rm_specperacnt",
                column: "endtime");

            migrationBuilder.CreateIndex(
                name: "srvid",
                table: "rm_specperacnt",
                column: "srvid");

            migrationBuilder.CreateIndex(
                name: "fromtime",
                table: "rm_specperacnt",
                column: "starttime");

            migrationBuilder.CreateIndex(
                name: "acctype",
                table: "rm_users",
                column: "acctype");

            migrationBuilder.CreateIndex(
                name: "address",
                table: "rm_users",
                column: "address");

            migrationBuilder.CreateIndex(
                name: "city",
                table: "rm_users",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "comment",
                table: "rm_users",
                column: "comment");

            migrationBuilder.CreateIndex(
                name: "company",
                table: "rm_users",
                column: "company");

            migrationBuilder.CreateIndex(
                name: "contractid",
                table: "rm_users",
                column: "contractid");

            migrationBuilder.CreateIndex(
                name: "contractvalid",
                table: "rm_users",
                column: "contractvalid");

            migrationBuilder.CreateIndex(
                name: "country",
                table: "rm_users",
                column: "country");

            migrationBuilder.CreateIndex(
                name: "createdon",
                table: "rm_users",
                column: "createdon");

            migrationBuilder.CreateIndex(
                name: "email",
                table: "rm_users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "enableuser",
                table: "rm_users",
                column: "enableuser");

            migrationBuilder.CreateIndex(
                name: "expiration",
                table: "rm_users",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "firstname",
                table: "rm_users",
                column: "firstname");

            migrationBuilder.CreateIndex(
                name: "groupid",
                table: "rm_users",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "lastlogoff",
                table: "rm_users",
                column: "lastlogoff");

            migrationBuilder.CreateIndex(
                name: "lastname",
                table: "rm_users",
                column: "lastname");

            migrationBuilder.CreateIndex(
                name: "mac",
                table: "rm_users",
                column: "mac");

            migrationBuilder.CreateIndex(
                name: "maccm",
                table: "rm_users",
                column: "maccm");

            migrationBuilder.CreateIndex(
                name: "mobile",
                table: "rm_users",
                column: "mobile");

            migrationBuilder.CreateIndex(
                name: "owner",
                table: "rm_users",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "phone",
                table: "rm_users",
                column: "phone");

            migrationBuilder.CreateIndex(
                name: "srvid",
                table: "rm_users",
                column: "srvid");

            migrationBuilder.CreateIndex(
                name: "state",
                table: "rm_users",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "staticipcm",
                table: "rm_users",
                column: "staticipcm");

            migrationBuilder.CreateIndex(
                name: "staticipcpe",
                table: "rm_users",
                column: "staticipcpe");

            migrationBuilder.CreateIndex(
                name: "zip",
                table: "rm_users",
                column: "zip");

            migrationBuilder.CreateIndex(
                name: "maccpe",
                table: "rm_wlan",
                column: "maccpe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nas");

            migrationBuilder.DropTable(
                name: "radacct");

            migrationBuilder.DropTable(
                name: "radcheck");

            migrationBuilder.DropTable(
                name: "radgroupcheck");

            migrationBuilder.DropTable(
                name: "radgroupreply");

            migrationBuilder.DropTable(
                name: "radippool");

            migrationBuilder.DropTable(
                name: "radpostauth");

            migrationBuilder.DropTable(
                name: "radreply");

            migrationBuilder.DropTable(
                name: "radusergroup");

            migrationBuilder.DropTable(
                name: "rm_actsrv");

            migrationBuilder.DropTable(
                name: "rm_allowedmanagers");

            migrationBuilder.DropTable(
                name: "rm_allowednases");

            migrationBuilder.DropTable(
                name: "rm_ap");

            migrationBuilder.DropTable(
                name: "rm_cards");

            migrationBuilder.DropTable(
                name: "rm_changesrv");

            migrationBuilder.DropTable(
                name: "rm_cmts");

            migrationBuilder.DropTable(
                name: "rm_colsetlistdocsis");

            migrationBuilder.DropTable(
                name: "rm_colsetlistradius");

            migrationBuilder.DropTable(
                name: "rm_colsetlistusers");

            migrationBuilder.DropTable(
                name: "rm_ias");

            migrationBuilder.DropTable(
                name: "rm_invoices");

            migrationBuilder.DropTable(
                name: "rm_ippools");

            migrationBuilder.DropTable(
                name: "rm_managers");

            migrationBuilder.DropTable(
                name: "rm_newusers");

            migrationBuilder.DropTable(
                name: "rm_onlinecm");

            migrationBuilder.DropTable(
                name: "rm_phpsess");

            migrationBuilder.DropTable(
                name: "rm_radacct");

            migrationBuilder.DropTable(
                name: "rm_services");

            migrationBuilder.DropTable(
                name: "rm_settings");

            migrationBuilder.DropTable(
                name: "rm_specperacnt");

            migrationBuilder.DropTable(
                name: "rm_specperbw");

            migrationBuilder.DropTable(
                name: "rm_syslog");

            migrationBuilder.DropTable(
                name: "rm_usergroups");

            migrationBuilder.DropTable(
                name: "rm_users");

            migrationBuilder.DropTable(
                name: "rm_wlan");
        }
    }
}
