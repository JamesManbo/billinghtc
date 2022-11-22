using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RadiusContext : DbContext
    {
        public RadiusContext()
        {
        }

        public RadiusContext(DbContextOptions<RadiusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Nas> Nas { get; set; }
        public virtual DbSet<Radacct> Radacct { get; set; }
        public virtual DbSet<Radcheck> Radcheck { get; set; }
        public virtual DbSet<Radgroupcheck> Radgroupcheck { get; set; }
        public virtual DbSet<Radgroupreply> Radgroupreply { get; set; }
        public virtual DbSet<Radippool> Radippool { get; set; }
        public virtual DbSet<Radpostauth> Radpostauth { get; set; }
        public virtual DbSet<Radreply> Radreply { get; set; }
        public virtual DbSet<Radusergroup> Radusergroup { get; set; }
        public virtual DbSet<RmActsrv> RmActsrv { get; set; }
        public virtual DbSet<RmAllowedmanagers> RmAllowedmanagers { get; set; }
        public virtual DbSet<RmAllowednases> RmAllowednases { get; set; }
        public virtual DbSet<RmAp> RmAp { get; set; }
        public virtual DbSet<RmCards> RmCards { get; set; }
        public virtual DbSet<RmChangesrv> RmChangesrv { get; set; }
        public virtual DbSet<RmCmts> RmCmts { get; set; }
        public virtual DbSet<RmColsetlistdocsis> RmColsetlistdocsis { get; set; }
        public virtual DbSet<RmColsetlistradius> RmColsetlistradius { get; set; }
        public virtual DbSet<RmColsetlistusers> RmColsetlistusers { get; set; }
        public virtual DbSet<RmIas> RmIas { get; set; }
        public virtual DbSet<RmInvoices> RmInvoices { get; set; }
        public virtual DbSet<RmIppools> RmIppools { get; set; }
        public virtual DbSet<RmManagers> RmManagers { get; set; }
        public virtual DbSet<RmNewusers> RmNewusers { get; set; }
        public virtual DbSet<RmOnlinecm> RmOnlinecm { get; set; }
        public virtual DbSet<RmPhpsess> RmPhpsess { get; set; }
        public virtual DbSet<RmRadacct> RmRadacct { get; set; }
        public virtual DbSet<RmServices> RmServices { get; set; }
        public virtual DbSet<RmSettings> RmSettings { get; set; }
        public virtual DbSet<RmSpecperacnt> RmSpecperacnt { get; set; }
        public virtual DbSet<RmSpecperbw> RmSpecperbw { get; set; }
        public virtual DbSet<RmSyslog> RmSyslog { get; set; }
        public virtual DbSet<RmUsergroups> RmUsergroups { get; set; }
        public virtual DbSet<RmUsers> RmUsers { get; set; }
        public virtual DbSet<RmWlan> RmWlan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=192.168.100.73;database=radius;user=dev;password=Dev@2021;persistsecurityinfo=True;allowloadlocalinfile=True;allowuservariables=True;Convert Zero Datetime=True;TreatTinyAsBoolean=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nas>(entity =>
            {
                entity.ToTable("nas");

                entity.HasIndex(e => e.Nasname)
                    .HasName("nasname");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Apipassword)
                    .IsRequired()
                    .HasColumnName("apipassword")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Apiusername)
                    .IsRequired()
                    .HasColumnName("apiusername")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ciscobwmode)
                    .HasColumnName("ciscobwmode")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Community)
                    .HasColumnName("community")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("'RADIUS Client'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Enableapi)
                    .HasColumnName("enableapi")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Nasname)
                    .IsRequired()
                    .HasColumnName("nasname")
                    .HasColumnType("varchar(128)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ports)
                    .HasColumnName("ports")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasColumnName("secret")
                    .HasColumnType("varchar(60)")
                    .HasDefaultValueSql("'secret'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Shortname)
                    .HasColumnName("shortname")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Starospassword)
                    .IsRequired()
                    .HasColumnName("starospassword")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(30)")
                    .HasDefaultValueSql("'other'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radacct>(entity =>
            {
                entity.ToTable("radacct");

                entity.HasIndex(e => e.Acctsessionid)
                    .HasName("acctsessionid");

                entity.HasIndex(e => e.Acctsessiontime)
                    .HasName("acctsessiontime");

                entity.HasIndex(e => e.Acctstarttime)
                    .HasName("acctstarttime");

                entity.HasIndex(e => e.Acctstoptime)
                    .HasName("acctstoptime");

                entity.HasIndex(e => e.Accttime)
                    .HasName("_AcctTime");

                entity.HasIndex(e => e.Acctuniqueid)
                    .HasName("acctuniqueid");

                entity.HasIndex(e => e.Callingstationid)
                    .HasName("callingstationid");

                entity.HasIndex(e => e.Framedipaddress)
                    .HasName("framedipaddress");

                entity.HasIndex(e => e.Nasipaddress)
                    .HasName("nasipaddress");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Radacctid)
                    .HasColumnName("radacctid")
                    .HasColumnType("bigint(21)");

                entity.Property(e => e.Acctauthentic)
                    .HasColumnName("acctauthentic")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Acctinputoctets)
                    .HasColumnName("acctinputoctets")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Acctoutputoctets)
                    .HasColumnName("acctoutputoctets")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Acctsessionid)
                    .IsRequired()
                    .HasColumnName("acctsessionid")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Acctsessiontime)
                    .HasColumnName("acctsessiontime")
                    .HasColumnType("int(12)");

                entity.Property(e => e.Acctstartdelay)
                    .HasColumnName("acctstartdelay")
                    .HasColumnType("int(12)");

                entity.Property(e => e.Acctstarttime).HasColumnName("acctstarttime");

                entity.Property(e => e.Acctstopdelay)
                    .HasColumnName("acctstopdelay")
                    .HasColumnType("int(12)");

                entity.Property(e => e.Acctstoptime).HasColumnName("acctstoptime");

                entity.Property(e => e.Acctterminatecause)
                    .IsRequired()
                    .HasColumnName("acctterminatecause")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Accttime).HasColumnName("_accttime");

                entity.Property(e => e.Acctuniqueid)
                    .IsRequired()
                    .HasColumnName("acctuniqueid")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Apid)
                    .HasColumnName("_apid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Calledstationid)
                    .IsRequired()
                    .HasColumnName("calledstationid")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Callingstationid)
                    .IsRequired()
                    .HasColumnName("callingstationid")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ConnectinfoStart)
                    .HasColumnName("connectinfo_start")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ConnectinfoStop)
                    .HasColumnName("connectinfo_stop")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Dailynextsrvactive)
                    .HasColumnName("_dailynextsrvactive")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Framedipaddress)
                    .IsRequired()
                    .HasColumnName("framedipaddress")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Framedprotocol)
                    .HasColumnName("framedprotocol")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nasipaddress)
                    .IsRequired()
                    .HasColumnName("nasipaddress")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nasportid)
                    .HasColumnName("nasportid")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nasporttype)
                    .HasColumnName("nasporttype")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Realm)
                    .HasColumnName("realm")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Servicetype)
                    .HasColumnName("servicetype")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Srvid)
                    .HasColumnName("_srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Xascendsessionsvrkey)
                    .HasColumnName("xascendsessionsvrkey")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radcheck>(entity =>
            {
                entity.ToTable("radcheck");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasColumnName("attribute")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Op)
                    .IsRequired()
                    .HasColumnName("op")
                    .HasColumnType("char(2)")
                    .HasDefaultValueSql("'=='")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(253)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radgroupcheck>(entity =>
            {
                entity.ToTable("radgroupcheck");

                entity.HasIndex(e => e.Groupname)
                    .HasName("groupname");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasColumnName("attribute")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Op)
                    .IsRequired()
                    .HasColumnName("op")
                    .HasColumnType("char(2)")
                    .HasDefaultValueSql("'=='")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(253)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radgroupreply>(entity =>
            {
                entity.ToTable("radgroupreply");

                entity.HasIndex(e => e.Groupname)
                    .HasName("groupname");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasColumnName("attribute")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Op)
                    .IsRequired()
                    .HasColumnName("op")
                    .HasColumnType("char(2)")
                    .HasDefaultValueSql("'='")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(253)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radippool>(entity =>
            {
                entity.ToTable("radippool");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Calledstationid)
                    .IsRequired()
                    .HasColumnName("calledstationid")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Callingstationid)
                    .IsRequired()
                    .HasColumnName("callingstationid")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ExpiryTime).HasColumnName("expiry_time");

                entity.Property(e => e.Framedipaddress)
                    .IsRequired()
                    .HasColumnName("framedipaddress")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nasipaddress)
                    .IsRequired()
                    .HasColumnName("nasipaddress")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PoolKey)
                    .IsRequired()
                    .HasColumnName("pool_key")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PoolName)
                    .IsRequired()
                    .HasColumnName("pool_name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radpostauth>(entity =>
            {
                entity.ToTable("radpostauth");

                entity.HasIndex(e => e.Authdate)
                    .HasName("authdate");

                entity.HasIndex(e => e.Nasipaddress)
                    .HasName("nasipaddress");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Authdate)
                    .HasColumnName("authdate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Nasipaddress)
                    .IsRequired()
                    .HasColumnName("nasipaddress")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasColumnName("pass")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Reply)
                    .IsRequired()
                    .HasColumnName("reply")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radreply>(entity =>
            {
                entity.ToTable("radreply");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11) unsigned");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasColumnName("attribute")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Op)
                    .IsRequired()
                    .HasColumnName("op")
                    .HasColumnType("char(2)")
                    .HasDefaultValueSql("'='")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(253)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Radusergroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("radusergroup");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Priority)
                    .HasColumnName("priority")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmActsrv>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("rm_actsrv");

                entity.HasIndex(e => e.Datetime)
                    .HasName("datetime");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Dailynextsrvactive)
                    .HasColumnName("dailynextsrvactive")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Datetime).HasColumnName("datetime");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmAllowedmanagers>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_allowedmanagers");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.HasIndex(e => e.Srvid)
                    .HasName("srvid");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<RmAllowednases>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_allowednases");

                entity.HasIndex(e => e.Nasid)
                    .HasName("nasid");

                entity.HasIndex(e => e.Srvid)
                    .HasName("srvid");

                entity.Property(e => e.Nasid)
                    .HasColumnName("nasid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<RmAp>(entity =>
            {
                entity.ToTable("rm_ap");

                entity.HasIndex(e => e.Ip)
                    .HasName("ip");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Accessmode)
                    .HasColumnName("accessmode")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Apipassword)
                    .IsRequired()
                    .HasColumnName("apipassword")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Apiusername)
                    .IsRequired()
                    .HasColumnName("apiusername")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Community)
                    .IsRequired()
                    .HasColumnName("community")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Enable)
                    .HasColumnName("enable")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmCards>(entity =>
            {
                entity.ToTable("rm_cards");

                entity.HasIndex(e => e.Cardnum)
                    .HasName("cardnum")
                    .IsUnique();

                entity.HasIndex(e => e.Owner)
                    .HasName("owner");

                entity.HasIndex(e => e.Series)
                    .HasName("series");

                entity.HasIndex(e => e.Used)
                    .HasName("used");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Cardnum)
                    .IsRequired()
                    .HasColumnName("cardnum")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cardtype)
                    .HasColumnName("cardtype")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Comblimit)
                    .HasColumnName("comblimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Downlimit)
                    .HasColumnName("downlimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Expiration)
                    .HasColumnName("expiration")
                    .HasColumnType("date");

                entity.Property(e => e.Expiretime)
                    .HasColumnName("expiretime")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasColumnName("owner")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Series)
                    .IsRequired()
                    .HasColumnName("series")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timebaseexp)
                    .HasColumnName("timebaseexp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timebaseonline)
                    .HasColumnName("timebaseonline")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Transid)
                    .IsRequired()
                    .HasColumnName("transid")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Uplimit)
                    .HasColumnName("uplimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Uptimelimit)
                    .HasColumnName("uptimelimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Used).HasColumnName("used");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("decimal(22,2)");
            });

            modelBuilder.Entity<RmChangesrv>(entity =>
            {
                entity.ToTable("rm_changesrv");

                entity.HasIndex(e => e.Requestdate)
                    .HasName("requestdate");

                entity.HasIndex(e => e.Scheduledate)
                    .HasName("scheduledate");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Newsrvid)
                    .HasColumnName("newsrvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Newsrvname)
                    .IsRequired()
                    .HasColumnName("newsrvname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Requestdate)
                    .HasColumnName("requestdate")
                    .HasColumnType("date");

                entity.Property(e => e.Requested)
                    .IsRequired()
                    .HasColumnName("requested")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Scheduledate)
                    .HasColumnName("scheduledate")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Transid)
                    .IsRequired()
                    .HasColumnName("transid")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmCmts>(entity =>
            {
                entity.ToTable("rm_cmts");

                entity.HasIndex(e => e.Ip)
                    .HasName("ip");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Community)
                    .IsRequired()
                    .HasColumnName("community")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnName("descr")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmColsetlistdocsis>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_colsetlistdocsis");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.Property(e => e.Colname)
                    .IsRequired()
                    .HasColumnName("colname")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmColsetlistradius>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_colsetlistradius");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.Property(e => e.Colname)
                    .IsRequired()
                    .HasColumnName("colname")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmColsetlistusers>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_colsetlistusers");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.Property(e => e.Colname)
                    .IsRequired()
                    .HasColumnName("colname")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmIas>(entity =>
            {
                entity.HasKey(e => e.Iasid)
                    .HasName("PRIMARY");

                entity.ToTable("rm_ias");

                entity.Property(e => e.Iasid)
                    .HasColumnName("iasid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Comblimit)
                    .HasColumnName("comblimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Downlimit)
                    .HasColumnName("downlimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Enableias)
                    .HasColumnName("enableias")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Expiration)
                    .HasColumnName("expiration")
                    .HasColumnType("date");

                entity.Property(e => e.Expiremode)
                    .HasColumnName("expiremode")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Expiretime)
                    .HasColumnName("expiretime")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Iasname)
                    .IsRequired()
                    .HasColumnName("iasname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.Simuse)
                    .HasColumnName("simuse")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timebaseexp)
                    .HasColumnName("timebaseexp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timebaseonline)
                    .HasColumnName("timebaseonline")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Uplimit)
                    .HasColumnName("uplimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Uptimelimit)
                    .HasColumnName("uptimelimit")
                    .HasColumnType("bigint(20)");
            });

            modelBuilder.Entity<RmInvoices>(entity =>
            {
                entity.ToTable("rm_invoices");

                entity.HasIndex(e => e.Comment)
                    .HasName("comment");

                entity.HasIndex(e => e.Date)
                    .HasName("date");

                entity.HasIndex(e => e.Gwtransid)
                    .HasName("gwtransid");

                entity.HasIndex(e => e.Invgroup)
                    .HasName("invgroup");

                entity.HasIndex(e => e.Invnum)
                    .HasName("invnum");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.HasIndex(e => e.Paid)
                    .HasName("paid");

                entity.HasIndex(e => e.Paymode)
                    .HasName("paymode");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("decimal(13,2)");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.Bytescomb)
                    .HasColumnName("bytescomb")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Bytesdl)
                    .HasColumnName("bytesdl")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Bytesul)
                    .HasColumnName("bytesul")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Capdate)
                    .HasColumnName("capdate")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Capdl)
                    .HasColumnName("capdl")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Captime)
                    .HasColumnName("captime")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Captotal)
                    .HasColumnName("captotal")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Capul)
                    .HasColumnName("capul")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comblimit)
                    .HasColumnName("comblimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Days)
                    .HasColumnName("days")
                    .HasColumnType("int(6)");

                entity.Property(e => e.Downlimit)
                    .HasColumnName("downlimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Expiration)
                    .HasColumnName("expiration")
                    .HasColumnType("date");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Gwtransid)
                    .IsRequired()
                    .HasColumnName("gwtransid")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Invgroup)
                    .HasColumnName("invgroup")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Invnum)
                    .IsRequired()
                    .HasColumnName("invnum")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Invtype)
                    .HasColumnName("invtype")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Paid)
                    .HasColumnName("paid")
                    .HasColumnType("date");

                entity.Property(e => e.Paymentopt)
                    .HasColumnName("paymentopt")
                    .HasColumnType("date");

                entity.Property(e => e.Paymode)
                    .HasColumnName("paymode")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasColumnName("remark")
                    .HasColumnType("varchar(400)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasColumnName("service")
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tax)
                    .HasColumnName("tax")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Taxid)
                    .IsRequired()
                    .HasColumnName("taxid")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Transid)
                    .IsRequired()
                    .HasColumnName("transid")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Uplimit)
                    .HasColumnName("uplimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Uptimelimit)
                    .HasColumnName("uptimelimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Vatpercent)
                    .HasColumnName("vatpercent")
                    .HasColumnType("decimal(4,2)");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmIppools>(entity =>
            {
                entity.ToTable("rm_ippools");

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.HasIndex(e => e.Nextpoolid)
                    .HasName("nextid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnName("descr")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Fromip)
                    .IsRequired()
                    .HasColumnName("fromip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nextpoolid)
                    .HasColumnName("nextpoolid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Toip)
                    .IsRequired()
                    .HasColumnName("toip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<RmManagers>(entity =>
            {
                entity.HasKey(e => e.Managername)
                    .HasName("PRIMARY");

                entity.ToTable("rm_managers");

                entity.Property(e => e.Managername)
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasColumnName("company")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Enablemanager)
                    .HasColumnName("enablemanager")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lang)
                    .IsRequired()
                    .HasColumnName("lang")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PermAccessap)
                    .HasColumnName("perm_accessap")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermAddcredits)
                    .HasColumnName("perm_addcredits")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermAllowdiscount)
                    .HasColumnName("perm_allowdiscount")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermAllusers)
                    .HasColumnName("perm_allusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermCardsys)
                    .HasColumnName("perm_cardsys")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermCreatemanagers)
                    .HasColumnName("perm_createmanagers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermCreateservices)
                    .HasColumnName("perm_createservices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermCreateusers)
                    .HasColumnName("perm_createusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermCts)
                    .HasColumnName("perm_cts")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermDeletemanagers)
                    .HasColumnName("perm_deletemanagers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermDeleteservices)
                    .HasColumnName("perm_deleteservices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermDeleteusers)
                    .HasColumnName("perm_deleteusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEditinvoice)
                    .HasColumnName("perm_editinvoice")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEditmanagers)
                    .HasColumnName("perm_editmanagers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEditservices)
                    .HasColumnName("perm_editservices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEditusers)
                    .HasColumnName("perm_editusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEdituserspriv)
                    .HasColumnName("perm_edituserspriv")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermEnwriteoff)
                    .HasColumnName("perm_enwriteoff")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListallinvoices)
                    .HasColumnName("perm_listallinvoices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListinvoices)
                    .HasColumnName("perm_listinvoices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListmanagers)
                    .HasColumnName("perm_listmanagers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListonlineusers)
                    .HasColumnName("perm_listonlineusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListservices)
                    .HasColumnName("perm_listservices")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermListusers)
                    .HasColumnName("perm_listusers")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermLogout)
                    .HasColumnName("perm_logout")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermNegbalance)
                    .HasColumnName("perm_negbalance")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermShowinvtotals)
                    .HasColumnName("perm_showinvtotals")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PermTrafficreport)
                    .HasColumnName("perm_trafficreport")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Vatid)
                    .IsRequired()
                    .HasColumnName("vatid")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmNewusers>(entity =>
            {
                entity.ToTable("rm_newusers");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Actcode)
                    .IsRequired()
                    .HasColumnName("actcode")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Actcount)
                    .HasColumnName("actcount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lang)
                    .IsRequired()
                    .HasColumnName("lang")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Vatid)
                    .IsRequired()
                    .HasColumnName("vatid")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmOnlinecm>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PRIMARY");

                entity.ToTable("rm_onlinecm");

                entity.HasIndex(e => e.Groupname)
                    .HasName("groupname");

                entity.HasIndex(e => e.Ipcpe)
                    .HasName("ipcpe");

                entity.HasIndex(e => e.Maccm)
                    .HasName("maccm");

                entity.HasIndex(e => e.Staticipcm)
                    .HasName("staticipcm");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cmtsid)
                    .HasColumnName("cmtsid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enableuser)
                    .HasColumnName("enableuser")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Groupname)
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ifidx)
                    .HasColumnName("ifidx")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ipcpe)
                    .HasColumnName("ipcpe")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ipmodecpe)
                    .HasColumnName("ipmodecpe")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Maccm)
                    .HasColumnName("maccm")
                    .HasColumnType("varchar(17)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Maccpe)
                    .HasColumnName("maccpe")
                    .HasColumnType("varchar(17)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pingtime)
                    .HasColumnName("pingtime")
                    .HasColumnType("decimal(11,1)");

                entity.Property(e => e.Rxpwr)
                    .HasColumnName("rxpwr")
                    .HasColumnType("decimal(11,1)");

                entity.Property(e => e.Snrds)
                    .HasColumnName("snrds")
                    .HasColumnType("decimal(11,1)");

                entity.Property(e => e.Snrus)
                    .HasColumnName("snrus")
                    .HasColumnType("decimal(11,1)");

                entity.Property(e => e.Staticipcm)
                    .HasColumnName("staticipcm")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Txpwr)
                    .HasColumnName("txpwr")
                    .HasColumnType("decimal(11,1)");

                entity.Property(e => e.Upstreamname)
                    .HasColumnName("upstreamname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmPhpsess>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_phpsess");

                entity.HasIndex(e => e.Managername)
                    .HasName("managername");

                entity.Property(e => e.Closed)
                    .HasColumnName("closed")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastact).HasColumnName("lastact");

                entity.Property(e => e.Managername)
                    .IsRequired()
                    .HasColumnName("managername")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Sessid)
                    .IsRequired()
                    .HasColumnName("sessid")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmRadacct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_radacct");

                entity.HasIndex(e => e.Acctstarttime)
                    .HasName("acctstarttime");

                entity.HasIndex(e => e.Acctstoptime)
                    .HasName("acctstoptime");

                entity.HasIndex(e => e.Acctuniqueid)
                    .HasName("acctuniqueid");

                entity.HasIndex(e => e.Radacctid)
                    .HasName("radacctid");

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.Acctsessiontime)
                    .HasColumnName("acctsessiontime")
                    .HasColumnType("int(12)");

                entity.Property(e => e.Acctsessiontimeratio)
                    .HasColumnName("acctsessiontimeratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Acctstarttime).HasColumnName("acctstarttime");

                entity.Property(e => e.Acctstoptime).HasColumnName("acctstoptime");

                entity.Property(e => e.Acctuniqueid)
                    .IsRequired()
                    .HasColumnName("acctuniqueid")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Dlbytes)
                    .HasColumnName("dlbytes")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Dlbytesstart)
                    .HasColumnName("dlbytesstart")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Dlbytesstop)
                    .HasColumnName("dlbytesstop")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Dlratio)
                    .HasColumnName("dlratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Radacctid)
                    .HasColumnName("radacctid")
                    .HasColumnType("bigint(21)");

                entity.Property(e => e.Ulbytes)
                    .HasColumnName("ulbytes")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Ulbytesstart)
                    .HasColumnName("ulbytesstart")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Ulbytesstop)
                    .HasColumnName("ulbytesstop")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Ulratio)
                    .HasColumnName("ulratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmServices>(entity =>
            {
                entity.HasKey(e => e.Srvid)
                    .HasName("PRIMARY");

                entity.ToTable("rm_services");

                entity.HasIndex(e => e.Srvname)
                    .HasName("srvname");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Addamount)
                    .HasColumnName("addamount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Advcmcfg)
                    .HasColumnName("advcmcfg")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Availucp)
                    .HasColumnName("availucp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Carryover)
                    .HasColumnName("carryover")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Cmcfg)
                    .IsRequired()
                    .HasColumnName("cmcfg")
                    .HasColumnType("varchar(10240)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Combquota)
                    .HasColumnName("combquota")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Custattr)
                    .IsRequired()
                    .HasColumnName("custattr")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Dailynextsrvid)
                    .HasColumnName("dailynextsrvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnName("descr")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Disnextsrvid)
                    .HasColumnName("disnextsrvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlburstlimit)
                    .HasColumnName("dlburstlimit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlburstthreshold)
                    .HasColumnName("dlburstthreshold")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlbursttime)
                    .HasColumnName("dlbursttime")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlquota)
                    .HasColumnName("dlquota")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Downrate)
                    .HasColumnName("downrate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enableburst)
                    .HasColumnName("enableburst")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Enableservice)
                    .HasColumnName("enableservice")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enaddcredits)
                    .HasColumnName("enaddcredits")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Gentftp)
                    .HasColumnName("gentftp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ignstatip)
                    .HasColumnName("ignstatip")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Initdl)
                    .HasColumnName("initdl")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inittimeexp)
                    .HasColumnName("inittimeexp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inittimeonline)
                    .HasColumnName("inittimeonline")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inittotal)
                    .HasColumnName("inittotal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Initul)
                    .HasColumnName("initul")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Limitcomb)
                    .HasColumnName("limitcomb")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Limitdl)
                    .HasColumnName("limitdl")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Limitexpiration)
                    .HasColumnName("limitexpiration")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Limitul)
                    .HasColumnName("limitul")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Limituptime)
                    .HasColumnName("limituptime")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Minamount)
                    .HasColumnName("minamount")
                    .HasColumnType("int(20)");

                entity.Property(e => e.Minamountadd)
                    .HasColumnName("minamountadd")
                    .HasColumnType("int(20)");

                entity.Property(e => e.Monthly)
                    .HasColumnName("monthly")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Nextsrvid)
                    .HasColumnName("nextsrvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Policymapdl)
                    .IsRequired()
                    .HasColumnName("policymapdl")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Policymapul)
                    .IsRequired()
                    .HasColumnName("policymapul")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Poolname)
                    .IsRequired()
                    .HasColumnName("poolname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pricecalcdownload)
                    .HasColumnName("pricecalcdownload")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Pricecalcupload)
                    .HasColumnName("pricecalcupload")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Pricecalcuptime)
                    .HasColumnName("pricecalcuptime")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Priority)
                    .HasColumnName("priority")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Renew)
                    .HasColumnName("renew")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Resetcounters)
                    .HasColumnName("resetcounters")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Srvname)
                    .IsRequired()
                    .HasColumnName("srvname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Srvtype)
                    .HasColumnName("srvtype")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timeaddmodeexp)
                    .HasColumnName("timeaddmodeexp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timeaddmodeonline)
                    .HasColumnName("timeaddmodeonline")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timebaseexp)
                    .HasColumnName("timebaseexp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timebaseonline)
                    .HasColumnName("timebaseonline")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timequota)
                    .HasColumnName("timequota")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Timeunitexp)
                    .HasColumnName("timeunitexp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timeunitonline)
                    .HasColumnName("timeunitonline")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Trafficaddmode)
                    .HasColumnName("trafficaddmode")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Trafficunitcomb)
                    .HasColumnName("trafficunitcomb")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Trafficunitdl)
                    .HasColumnName("trafficunitdl")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Trafficunitul)
                    .HasColumnName("trafficunitul")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulburstlimit)
                    .HasColumnName("ulburstlimit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulburstthreshold)
                    .HasColumnName("ulburstthreshold")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulbursttime)
                    .HasColumnName("ulbursttime")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulquota)
                    .HasColumnName("ulquota")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Unitprice)
                    .HasColumnName("unitprice")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Unitpriceadd)
                    .HasColumnName("unitpriceadd")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Unitpriceaddtax)
                    .HasColumnName("unitpriceaddtax")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Unitpricetax)
                    .HasColumnName("unitpricetax")
                    .HasColumnType("decimal(25,6)");

                entity.Property(e => e.Uprate)
                    .HasColumnName("uprate")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<RmSettings>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_settings");

                entity.Property(e => e.Billingstart)
                    .HasColumnName("billingstart")
                    .HasColumnType("tinyint(2)");

                entity.Property(e => e.Buycreditsucp)
                    .HasColumnName("buycreditsucp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Changepswucp)
                    .HasColumnName("changepswucp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Changesrv)
                    .HasColumnName("changesrv")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnName("currency")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Disableexpcont)
                    .HasColumnName("disableexpcont")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Disablenotpaid)
                    .HasColumnName("disablenotpaid")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Disconnmethod)
                    .HasColumnName("disconnmethod")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Diskquota)
                    .HasColumnName("diskquota")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Edituserdata)
                    .HasColumnName("edituserdata")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Emailexpiry)
                    .HasColumnName("emailexpiry")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Emailnewsrv)
                    .HasColumnName("emailnewsrv")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Emailrenew)
                    .HasColumnName("emailrenew")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Emailselfregman)
                    .HasColumnName("emailselfregman")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Emailwelcome)
                    .HasColumnName("emailwelcome")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Hidelimits)
                    .HasColumnName("hidelimits")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IasEmail)
                    .HasColumnName("ias_email")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IasEndupemail)
                    .HasColumnName("ias_endupemail")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IasEndupmobile)
                    .HasColumnName("ias_endupmobile")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IasMobile)
                    .HasColumnName("ias_mobile")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IasVerify)
                    .HasColumnName("ias_verify")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Maclock)
                    .HasColumnName("maclock")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Newmanallsrv)
                    .HasColumnName("newmanallsrv")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Newnasallsrv)
                    .HasColumnName("newnasallsrv")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Paymentopt)
                    .HasColumnName("paymentopt")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pm2co)
                    .HasColumnName("pm_2co")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmAuthorizenet)
                    .HasColumnName("pm_authorizenet")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmDps)
                    .HasColumnName("pm_dps")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmInternal)
                    .HasColumnName("pm_internal")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmPayfast)
                    .HasColumnName("pm_payfast")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmPaypalexp)
                    .HasColumnName("pm_paypalexp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmPaypalpro)
                    .HasColumnName("pm_paypalpro")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmPaypalstd)
                    .HasColumnName("pm_paypalstd")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.PmSagepay)
                    .HasColumnName("pm_sagepay")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Quotatpl)
                    .IsRequired()
                    .HasColumnName("quotatpl")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Redeemucp)
                    .HasColumnName("redeemucp")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Remotehostname)
                    .IsRequired()
                    .HasColumnName("remotehostname")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Renewday)
                    .HasColumnName("renewday")
                    .HasColumnType("tinyint(2)");

                entity.Property(e => e.Resetctr)
                    .HasColumnName("resetctr")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Selfreg)
                    .HasColumnName("selfreg")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregAddress)
                    .HasColumnName("selfreg_address")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregCity)
                    .HasColumnName("selfreg_city")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregCountry)
                    .HasColumnName("selfreg_country")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregEmail)
                    .HasColumnName("selfreg_email")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregEndupemail)
                    .HasColumnName("selfreg_endupemail")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregEndupmobile)
                    .HasColumnName("selfreg_endupmobile")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregFirstname)
                    .HasColumnName("selfreg_firstname")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregLastname)
                    .HasColumnName("selfreg_lastname")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregMobactsms)
                    .HasColumnName("selfreg_mobactsms")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregMobile)
                    .HasColumnName("selfreg_mobile")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregNameactemail)
                    .HasColumnName("selfreg_nameactemail")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregNameactsms)
                    .HasColumnName("selfreg_nameactsms")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregPhone)
                    .HasColumnName("selfreg_phone")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregState)
                    .HasColumnName("selfreg_state")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregVatid)
                    .HasColumnName("selfreg_vatid")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.SelfregZip)
                    .HasColumnName("selfreg_zip")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Simuseselfreg)
                    .HasColumnName("simuseselfreg")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Smsexpiry)
                    .HasColumnName("smsexpiry")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Smswelcome)
                    .HasColumnName("smswelcome")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Unixacc)
                    .HasColumnName("unixacc")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Unixhost)
                    .HasColumnName("unixhost")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Vatpercent)
                    .HasColumnName("vatpercent")
                    .HasColumnType("decimal(4,2)");

                entity.Property(e => e.Warncomb)
                    .HasColumnName("warncomb")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Warncombpercent)
                    .HasColumnName("warncombpercent")
                    .HasColumnType("int(3)");

                entity.Property(e => e.Warndl)
                    .HasColumnName("warndl")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Warndlpercent)
                    .HasColumnName("warndlpercent")
                    .HasColumnType("int(3)");

                entity.Property(e => e.Warnexpiry)
                    .HasColumnName("warnexpiry")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Warnmode)
                    .HasColumnName("warnmode")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Warnul)
                    .HasColumnName("warnul")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Warnulpercent)
                    .HasColumnName("warnulpercent")
                    .HasColumnType("int(3)");

                entity.Property(e => e.Warnuptime)
                    .HasColumnName("warnuptime")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Warnuptimepercent)
                    .HasColumnName("warnuptimepercent")
                    .HasColumnType("int(3)");
            });

            modelBuilder.Entity<RmSpecperacnt>(entity =>
            {
                entity.ToTable("rm_specperacnt");

                entity.HasIndex(e => e.Endtime)
                    .HasName("totime");

                entity.HasIndex(e => e.Srvid)
                    .HasName("srvid");

                entity.HasIndex(e => e.Starttime)
                    .HasName("fromtime");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Connallowed)
                    .HasColumnName("connallowed")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Dlratio)
                    .HasColumnName("dlratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Endtime).HasColumnName("endtime");

                entity.Property(e => e.Fri)
                    .HasColumnName("fri")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Mon)
                    .HasColumnName("mon")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Sat)
                    .HasColumnName("sat")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Starttime).HasColumnName("starttime");

                entity.Property(e => e.Sun)
                    .HasColumnName("sun")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Thu)
                    .HasColumnName("thu")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Timeratio)
                    .HasColumnName("timeratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Tue)
                    .HasColumnName("tue")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ulratio)
                    .HasColumnName("ulratio")
                    .HasColumnType("decimal(3,2)");

                entity.Property(e => e.Wed)
                    .HasColumnName("wed")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<RmSpecperbw>(entity =>
            {
                entity.ToTable("rm_specperbw");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlburstlimit)
                    .HasColumnName("dlburstlimit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlburstthreshold)
                    .HasColumnName("dlburstthreshold")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlbursttime)
                    .HasColumnName("dlbursttime")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dlrate)
                    .HasColumnName("dlrate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enableburst)
                    .HasColumnName("enableburst")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Endtime).HasColumnName("endtime");

                entity.Property(e => e.Fri)
                    .HasColumnName("fri")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Mon)
                    .HasColumnName("mon")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Priority)
                    .HasColumnName("priority")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sat)
                    .HasColumnName("sat")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Starttime).HasColumnName("starttime");

                entity.Property(e => e.Sun)
                    .HasColumnName("sun")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Thu)
                    .HasColumnName("thu")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Tue)
                    .HasColumnName("tue")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ulburstlimit)
                    .HasColumnName("ulburstlimit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulburstthreshold)
                    .HasColumnName("ulburstthreshold")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulbursttime)
                    .HasColumnName("ulbursttime")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ulrate)
                    .HasColumnName("ulrate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Wed)
                    .HasColumnName("wed")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<RmSyslog>(entity =>
            {
                entity.ToTable("rm_syslog");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Data1)
                    .IsRequired()
                    .HasColumnName("data1")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Datetime).HasColumnName("datetime");

                entity.Property(e => e.Eventid)
                    .HasColumnName("eventid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmUsergroups>(entity =>
            {
                entity.HasKey(e => e.Groupid)
                    .HasName("PRIMARY");

                entity.ToTable("rm_usergroups");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnName("descr")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Groupname)
                    .IsRequired()
                    .HasColumnName("groupname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmUsers>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PRIMARY");

                entity.ToTable("rm_users");

                entity.HasIndex(e => e.Acctype)
                    .HasName("acctype");

                entity.HasIndex(e => e.Address)
                    .HasName("address");

                entity.HasIndex(e => e.City)
                    .HasName("city");

                entity.HasIndex(e => e.Comment)
                    .HasName("comment");

                entity.HasIndex(e => e.Company)
                    .HasName("company");

                entity.HasIndex(e => e.Contractid)
                    .HasName("contractid");

                entity.HasIndex(e => e.Contractvalid)
                    .HasName("contractvalid");

                entity.HasIndex(e => e.Country)
                    .HasName("country");

                entity.HasIndex(e => e.Createdon)
                    .HasName("createdon");

                entity.HasIndex(e => e.Email)
                    .HasName("email");

                entity.HasIndex(e => e.Enableuser)
                    .HasName("enableuser");

                entity.HasIndex(e => e.Expiration)
                    .HasName("expiration");

                entity.HasIndex(e => e.Firstname)
                    .HasName("firstname");

                entity.HasIndex(e => e.Groupid)
                    .HasName("groupid");

                entity.HasIndex(e => e.Lastlogoff)
                    .HasName("lastlogoff");

                entity.HasIndex(e => e.Lastname)
                    .HasName("lastname");

                entity.HasIndex(e => e.Mac)
                    .HasName("mac");

                entity.HasIndex(e => e.Maccm)
                    .HasName("maccm");

                entity.HasIndex(e => e.Mobile)
                    .HasName("mobile");

                entity.HasIndex(e => e.Owner)
                    .HasName("owner");

                entity.HasIndex(e => e.Phone)
                    .HasName("phone");

                entity.HasIndex(e => e.Srvid)
                    .HasName("srvid");

                entity.HasIndex(e => e.State)
                    .HasName("state");

                entity.HasIndex(e => e.Staticipcm)
                    .HasName("staticipcm");

                entity.HasIndex(e => e.Staticipcpe)
                    .HasName("staticipcpe");

                entity.HasIndex(e => e.Zip)
                    .HasName("zip");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Acctype)
                    .HasColumnName("acctype")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Actcode)
                    .IsRequired()
                    .HasColumnName("actcode")
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Alertemail)
                    .HasColumnName("alertemail")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Alertsms)
                    .HasColumnName("alertsms")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Cardfails)
                    .HasColumnName("cardfails")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comblimit)
                    .HasColumnName("comblimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasColumnName("company")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Contractid)
                    .IsRequired()
                    .HasColumnName("contractid")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Contractvalid)
                    .HasColumnName("contractvalid")
                    .HasColumnType("date");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Createdby)
                    .IsRequired()
                    .HasColumnName("createdby")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Createdon)
                    .HasColumnName("createdon")
                    .HasColumnType("date");

                entity.Property(e => e.Credits)
                    .HasColumnName("credits")
                    .HasColumnType("decimal(20,2)");

                entity.Property(e => e.Custattr)
                    .IsRequired()
                    .HasColumnName("custattr")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Downlimit)
                    .HasColumnName("downlimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Enableuser)
                    .HasColumnName("enableuser")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Expiration).HasColumnName("expiration");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Gpslat)
                    .HasColumnName("gpslat")
                    .HasColumnType("decimal(17,14)");

                entity.Property(e => e.Gpslong)
                    .HasColumnName("gpslong")
                    .HasColumnType("decimal(17,14)");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ipmodecm)
                    .HasColumnName("ipmodecm")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Ipmodecpe)
                    .HasColumnName("ipmodecpe")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Lang)
                    .IsRequired()
                    .HasColumnName("lang")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastlogoff).HasColumnName("lastlogoff");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("mac")
                    .HasColumnType("varchar(17)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Maccm)
                    .IsRequired()
                    .HasColumnName("maccm")
                    .HasColumnType("varchar(17)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasColumnName("owner")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Poolidcm)
                    .HasColumnName("poolidcm")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Poolidcpe)
                    .HasColumnName("poolidcpe")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pswactsmsnum)
                    .HasColumnName("pswactsmsnum")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Selfreg)
                    .HasColumnName("selfreg")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Srvid)
                    .HasColumnName("srvid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Staticipcm)
                    .IsRequired()
                    .HasColumnName("staticipcm")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Staticipcpe)
                    .IsRequired()
                    .HasColumnName("staticipcpe")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Taxid)
                    .IsRequired()
                    .HasColumnName("taxid")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Uplimit)
                    .HasColumnName("uplimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Uptimelimit)
                    .HasColumnName("uptimelimit")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Usemacauth)
                    .HasColumnName("usemacauth")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Verified)
                    .HasColumnName("verified")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Verifycode)
                    .IsRequired()
                    .HasColumnName("verifycode")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Verifyfails)
                    .HasColumnName("verifyfails")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Verifymobile)
                    .IsRequired()
                    .HasColumnName("verifymobile")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Verifysentnum)
                    .HasColumnName("verifysentnum")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Warningsent)
                    .HasColumnName("warningsent")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RmWlan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rm_wlan");

                entity.HasIndex(e => e.Maccpe)
                    .HasName("maccpe");

                entity.Property(e => e.Apip)
                    .HasColumnName("apip")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ccq)
                    .HasColumnName("ccq")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Maccpe)
                    .HasColumnName("maccpe")
                    .HasColumnType("varchar(17)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Signal)
                    .HasColumnName("signal")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Snr)
                    .HasColumnName("snr")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
