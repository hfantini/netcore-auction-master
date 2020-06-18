using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuctionMaster.App.Model
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auction> Auction { get; set; }
        public virtual DbSet<ConnectedRealm> ConnectedRealm { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemQuality> ItemQuality { get; set; }
        public virtual DbSet<Realm> Realm { get; set; }
        public virtual DbSet<RealmPopulation> RealmPopulation { get; set; }
        public virtual DbSet<RealmRegion> RealmRegion { get; set; }
        public virtual DbSet<ScheduledTask> ScheduledTask { get; set; }
        public virtual DbSet<ScheduledTaskFrequency> ScheduledTaskFrequency { get; set; }
        public virtual DbSet<ScheduledTaskInterval> ScheduledTaskInterval { get; set; }
        public virtual DbSet<ScheduledTaskType> ScheduledTaskType { get; set; }
        public virtual DbSet<TaskAuctionScan> TaskAuctionScan { get; set; }
        public virtual DbSet<TaskAuctionScanLog> TaskAuctionScanLog { get; set; }
        public virtual DbSet<TaskRealmScan> TaskRealmScan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=auction_master;uid=root;pwd=123456", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>(entity =>
            {
                entity.HasKey(e => e.AucId)
                    .HasName("PRIMARY");

                entity.ToTable("auction");

                entity.HasIndex(e => e.Item)
                    .HasName("fk_AUCTION_ITEM1_idx");

                entity.HasIndex(e => e.TaskAuctionScan)
                    .HasName("fk_AUCTION_AUCTION_SCAN1_idx");

                entity.Property(e => e.AucId).HasColumnName("AUC_ID");

                entity.Property(e => e.AucBid)
                    .HasColumnName("AUC_BID")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.AucBuyout).HasColumnName("AUC_BUYOUT");

                entity.Property(e => e.AucQuantity).HasColumnName("AUC_QUANTITY");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.TaskAuctionScan).HasColumnName("TASK_AUCTION_SCAN");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_ITEM1");

                entity.HasOne(d => d.TaskAuctionScanNavigation)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.TaskAuctionScan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_AUCTION_SCAN1");
            });

            modelBuilder.Entity<ConnectedRealm>(entity =>
            {
                entity.HasKey(e => e.CreId)
                    .HasName("PRIMARY");

                entity.ToTable("connected_realm");

                entity.HasIndex(e => e.RealmPopulation)
                    .HasName("fk_CONNECTED_REALM_REALM_POPULATION1_idx");

                entity.HasIndex(e => e.RealmRegion)
                    .HasName("fk_CONNECTED_REALM_REALM_REGION_idx");

                entity.Property(e => e.CreId).HasColumnName("CRE_ID");

                entity.Property(e => e.RealmPopulation).HasColumnName("REALM_POPULATION");

                entity.Property(e => e.RealmRegion).HasColumnName("REALM_REGION");

                entity.HasOne(d => d.RealmPopulationNavigation)
                    .WithMany(p => p.ConnectedRealm)
                    .HasForeignKey(d => d.RealmPopulation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CONNECTED_REALM_REALM_POPULATION1");

                entity.HasOne(d => d.RealmRegionNavigation)
                    .WithMany(p => p.ConnectedRealm)
                    .HasForeignKey(d => d.RealmRegion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CONNECTED_REALM_REALM_REGION");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItmId)
                    .HasName("PRIMARY");

                entity.ToTable("item");

                entity.HasIndex(e => e.ItemQuality)
                    .HasName("fk_ITEM_ITEM_QUALITY1_idx");

                entity.Property(e => e.ItmId).HasColumnName("ITM_ID");

                entity.Property(e => e.ItemQuality).HasColumnName("ITEM_QUALITY");

                entity.Property(e => e.ItmIcon)
                    .IsRequired()
                    .HasColumnName("ITM_ICON")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ItmLevelreq).HasColumnName("ITM_LEVELREQ");

                entity.Property(e => e.ItmName)
                    .IsRequired()
                    .HasColumnName("ITM_NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ItmPurchasePrice).HasColumnName("ITM_PURCHASE_PRICE");

                entity.Property(e => e.ItmSellPrice).HasColumnName("ITM_SELL_PRICE");

                entity.Property(e => e.ItmStackable).HasColumnName("ITM_STACKABLE");

                entity.HasOne(d => d.ItemQualityNavigation)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ItemQuality)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ITEM_ITEM_QUALITY1");
            });

            modelBuilder.Entity<ItemQuality>(entity =>
            {
                entity.HasKey(e => e.ItmqId)
                    .HasName("PRIMARY");

                entity.ToTable("item_quality");

                entity.Property(e => e.ItmqId).HasColumnName("ITMQ_ID");

                entity.Property(e => e.ItmqName)
                    .IsRequired()
                    .HasColumnName("ITMQ_NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Realm>(entity =>
            {
                entity.HasKey(e => e.ReaId)
                    .HasName("PRIMARY");

                entity.ToTable("realm");

                entity.HasIndex(e => e.ConnectedRealm)
                    .HasName("fk_REALM_CONNECTED_REALM1_idx");

                entity.Property(e => e.ReaId).HasColumnName("REA_ID");

                entity.Property(e => e.ConnectedRealm).HasColumnName("CONNECTED_REALM");

                entity.Property(e => e.ReaCategory)
                    .IsRequired()
                    .HasColumnName("REA_CATEGORY")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ReaLocale)
                    .IsRequired()
                    .HasColumnName("REA_LOCALE")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ReaName)
                    .IsRequired()
                    .HasColumnName("REA_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ReaTimezone)
                    .IsRequired()
                    .HasColumnName("REA_TIMEZONE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ConnectedRealmNavigation)
                    .WithMany(p => p.Realm)
                    .HasForeignKey(d => d.ConnectedRealm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_REALM_CONNECTED_REALM1");
            });

            modelBuilder.Entity<RealmPopulation>(entity =>
            {
                entity.HasKey(e => e.RpopId)
                    .HasName("PRIMARY");

                entity.ToTable("realm_population");

                entity.Property(e => e.RpopId).HasColumnName("RPOP_ID");

                entity.Property(e => e.RpopName)
                    .IsRequired()
                    .HasColumnName("RPOP_NAME")
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RealmRegion>(entity =>
            {
                entity.HasKey(e => e.RreId)
                    .HasName("PRIMARY");

                entity.ToTable("realm_region");

                entity.Property(e => e.RreId).HasColumnName("RRE_ID");

                entity.Property(e => e.RreName)
                    .IsRequired()
                    .HasColumnName("RRE_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ScheduledTask>(entity =>
            {
                entity.HasKey(e => e.StaId)
                    .HasName("PRIMARY");

                entity.ToTable("scheduled_task");

                entity.HasIndex(e => e.ScheduledTaskType)
                    .HasName("fk_SCHEDULED_TASK_SCHEDULED_TASK_TYPE1_idx");

                entity.HasIndex(e => e.SheduledTaskFrequency)
                    .HasName("fk_SCHEDULED_TASK_SHEDULED_TASK_FREQUENCY1_idx");

                entity.Property(e => e.StaId).HasColumnName("STA_ID");

                entity.Property(e => e.ScheduledTaskType).HasColumnName("SCHEDULED_TASK_TYPE");

                entity.Property(e => e.SheduledTaskFrequency).HasColumnName("SHEDULED_TASK_FREQUENCY");

                entity.Property(e => e.StaName)
                    .IsRequired()
                    .HasColumnName("STA_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ScheduledTaskTypeNavigation)
                    .WithMany(p => p.ScheduledTask)
                    .HasForeignKey(d => d.ScheduledTaskType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SCHEDULED_TASK_SCHEDULED_TASK_TYPE1");

                entity.HasOne(d => d.SheduledTaskFrequencyNavigation)
                    .WithMany(p => p.ScheduledTask)
                    .HasForeignKey(d => d.SheduledTaskFrequency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SCHEDULED_TASK_SHEDULED_TASK_FREQUENCY1");
            });

            modelBuilder.Entity<ScheduledTaskFrequency>(entity =>
            {
                entity.HasKey(e => e.StfId)
                    .HasName("PRIMARY");

                entity.ToTable("scheduled_task_frequency");

                entity.Property(e => e.StfId).HasColumnName("STF_ID");

                entity.Property(e => e.StfNome)
                    .HasColumnName("STF_NOME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ScheduledTaskInterval>(entity =>
            {
                entity.HasKey(e => e.StiId)
                    .HasName("PRIMARY");

                entity.ToTable("scheduled_task_interval");

                entity.HasIndex(e => e.ScheduledTask)
                    .HasName("fk_SCHEDULED_TASK_INTERVAL_SCHEDULED_TASK1_idx");

                entity.Property(e => e.StiId).HasColumnName("STI_ID");

                entity.Property(e => e.ScheduledTask).HasColumnName("SCHEDULED_TASK");

                entity.Property(e => e.StiInterval).HasColumnName("STI_INTERVAL");

                entity.Property(e => e.StiLastexecution)
                    .HasColumnName("STI_LASTEXECUTION")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.ScheduledTaskNavigation)
                    .WithMany(p => p.ScheduledTaskInterval)
                    .HasForeignKey(d => d.ScheduledTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SCHEDULED_TASK_INTERVAL_SCHEDULED_TASK1");
            });

            modelBuilder.Entity<ScheduledTaskType>(entity =>
            {
                entity.HasKey(e => e.SttId)
                    .HasName("PRIMARY");

                entity.ToTable("scheduled_task_type");

                entity.Property(e => e.SttId).HasColumnName("STT_ID");

                entity.Property(e => e.SttClass)
                    .HasColumnName("STT_CLASS")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SttName)
                    .HasColumnName("STT_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<TaskAuctionScan>(entity =>
            {
                entity.HasKey(e => e.TasId)
                    .HasName("PRIMARY");

                entity.ToTable("task_auction_scan");

                entity.HasIndex(e => e.ConnectedRealm)
                    .HasName("fk_AUCTION_SCAN_CONNECTED_REALM1_idx");

                entity.HasIndex(e => e.ScheduledTask)
                    .HasName("fk_TASK_AUCTION_SCAN_SCHEDULED_TASK1_idx");

                entity.Property(e => e.TasId).HasColumnName("TAS_ID");

                entity.Property(e => e.ConnectedRealm).HasColumnName("CONNECTED_REALM");

                entity.Property(e => e.ScheduledTask).HasColumnName("SCHEDULED_TASK");

                entity.Property(e => e.TasEndtime)
                    .HasColumnName("TAS_ENDTIME")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TasStarttime)
                    .HasColumnName("TAS_STARTTIME")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TasStatus).HasColumnName("TAS_STATUS");

                entity.HasOne(d => d.ConnectedRealmNavigation)
                    .WithMany(p => p.TaskAuctionScan)
                    .HasForeignKey(d => d.ConnectedRealm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_SCAN_CONNECTED_REALM1");

                entity.HasOne(d => d.ScheduledTaskNavigation)
                    .WithMany(p => p.TaskAuctionScan)
                    .HasForeignKey(d => d.ScheduledTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TASK_AUCTION_SCAN_SCHEDULED_TASK1");
            });

            modelBuilder.Entity<TaskAuctionScanLog>(entity =>
            {
                entity.HasKey(e => e.TaslId)
                    .HasName("PRIMARY");

                entity.ToTable("task_auction_scan_log");

                entity.HasIndex(e => e.TaskAuctionScan)
                    .HasName("fk_AUCTION_SCAN_LOG_AUCTION_SCAN1_idx");

                entity.Property(e => e.TaslId).HasColumnName("TASL_ID");

                entity.Property(e => e.TaskAuctionScan).HasColumnName("TASK_AUCTION_SCAN");

                entity.Property(e => e.TaslCode)
                    .HasColumnName("TASL_CODE")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.TaslMessage)
                    .IsRequired()
                    .HasColumnName("TASL_MESSAGE")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.TaskAuctionScanNavigation)
                    .WithMany(p => p.TaskAuctionScanLog)
                    .HasForeignKey(d => d.TaskAuctionScan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_SCAN_LOG_AUCTION_SCAN1");
            });

            modelBuilder.Entity<TaskRealmScan>(entity =>
            {
                entity.HasKey(e => e.TrsId)
                    .HasName("PRIMARY");

                entity.ToTable("task_realm_scan");

                entity.HasIndex(e => e.ScheduledTask)
                    .HasName("fk_TASK_REALM_SCAN_SCHEDULED_TASK1_idx");

                entity.Property(e => e.TrsId).HasColumnName("TRS_ID");

                entity.Property(e => e.ScheduledTask).HasColumnName("SCHEDULED_TASK");

                entity.Property(e => e.TrsCreCount).HasColumnName("TRS_CRE_COUNT");

                entity.Property(e => e.TrsEndtime)
                    .HasColumnName("TRS_ENDTIME")
                    .HasColumnType("timestamp");

                entity.Property(e => e.TrsReaCount).HasColumnName("TRS_REA_COUNT");

                entity.Property(e => e.TrsStarttime)
                    .HasColumnName("TRS_STARTTIME")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.ScheduledTaskNavigation)
                    .WithMany(p => p.TaskRealmScan)
                    .HasForeignKey(d => d.ScheduledTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TASK_REALM_SCAN_SCHEDULED_TASK1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
