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
        public virtual DbSet<ConnectedRealmRegion> ConnectedRealmRegion { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemQuality> ItemQuality { get; set; }
        public virtual DbSet<Realm> Realm { get; set; }
        public virtual DbSet<ScheduledTask> ScheduledTask { get; set; }
        public virtual DbSet<ScheduledTaskFrequency> ScheduledTaskFrequency { get; set; }
        public virtual DbSet<ScheduledTaskInterval> ScheduledTaskInterval { get; set; }
        public virtual DbSet<ScheduledTaskLog> ScheduledTaskLog { get; set; }
        public virtual DbSet<ScheduledTaskType> ScheduledTaskType { get; set; }

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
                entity.ToTable("auction");

                entity.HasIndex(e => e.ConnectedRealm)
                    .HasName("fk_AUCTION_CONNECTED_REALM1_idx");

                entity.HasIndex(e => e.Item)
                    .HasName("fk_AUCTION_ITEM1_idx");

                entity.HasIndex(e => e.ScheduledTaskLog)
                    .HasName("fk_AUCTION_SCHEDULED_TASK_LOG1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bid)
                    .HasColumnName("BID")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.Buyout).HasColumnName("BUYOUT");

                entity.Property(e => e.ConnectedRealm).HasColumnName("CONNECTED_REALM");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.Property(e => e.ScheduledTaskLog).HasColumnName("SCHEDULED_TASK_LOG");

                entity.HasOne(d => d.ConnectedRealmNavigation)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.ConnectedRealm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_CONNECTED_REALM1");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_ITEM1");

                entity.HasOne(d => d.ScheduledTaskLogNavigation)
                    .WithMany(p => p.Auction)
                    .HasForeignKey(d => d.ScheduledTaskLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AUCTION_SCHEDULED_TASK_LOG1");
            });

            modelBuilder.Entity<ConnectedRealm>(entity =>
            {
                entity.ToTable("connected_realm");

                entity.HasIndex(e => e.RealmRegion)
                    .HasName("fk_CONNECTED_REALM_REALM_REGION_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RealmRegion).HasColumnName("REALM_REGION");

                entity.HasOne(d => d.RealmRegionNavigation)
                    .WithMany(p => p.ConnectedRealm)
                    .HasForeignKey(d => d.RealmRegion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CONNECTED_REALM_REALM_REGION");
            });

            modelBuilder.Entity<ConnectedRealmRegion>(entity =>
            {
                entity.ToTable("connected_realm_region");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("CODE")
                    .HasColumnType("varchar(3)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.Quality)
                    .HasName("fk_ITEM_ITEM_QUALITY1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnName("ICON")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Levelreq).HasColumnName("LEVELREQ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PurchasePrice).HasColumnName("PURCHASE_PRICE");

                entity.Property(e => e.Quality).HasColumnName("QUALITY");

                entity.Property(e => e.SellPrice).HasColumnName("SELL_PRICE");

                entity.Property(e => e.Stackable).HasColumnName("STACKABLE");

                entity.HasOne(d => d.QualityNavigation)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.Quality)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ITEM_ITEM_QUALITY1");
            });

            modelBuilder.Entity<ItemQuality>(entity =>
            {
                entity.ToTable("item_quality");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Realm>(entity =>
            {
                entity.ToTable("realm");

                entity.HasIndex(e => e.ConnectedRealm)
                    .HasName("fk_REALM_CONNECTED_REALM1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnName("CATEGORY")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ConnectedRealm).HasColumnName("CONNECTED_REALM");

                entity.Property(e => e.Locale)
                    .IsRequired()
                    .HasColumnName("LOCALE")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Timezone)
                    .IsRequired()
                    .HasColumnName("TIMEZONE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ConnectedRealmNavigation)
                    .WithMany(p => p.Realm)
                    .HasForeignKey(d => d.ConnectedRealm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_REALM_CONNECTED_REALM1");
            });

            modelBuilder.Entity<ScheduledTask>(entity =>
            {
                entity.ToTable("scheduled_task");

                entity.HasIndex(e => e.ScheduledTaskType)
                    .HasName("fk_SCHEDULED_TASK_SCHEDULED_TASK_TYPE1_idx");

                entity.HasIndex(e => e.SheduledTaskFrequency)
                    .HasName("fk_SCHEDULED_TASK_SHEDULED_TASK_FREQUENCY1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Enabled).HasColumnName("ENABLED");

                entity.Property(e => e.MaxTentatives)
                    .HasColumnName("MAX_TENTATIVES")
                    .HasDefaultValueSql("'3'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Param)
                    .HasColumnName("PARAM")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ScheduledTaskType).HasColumnName("SCHEDULED_TASK_TYPE");

                entity.Property(e => e.SheduledTaskFrequency).HasColumnName("SHEDULED_TASK_FREQUENCY");

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
                entity.ToTable("scheduled_task_frequency");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nome)
                    .HasColumnName("NOME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ScheduledTaskInterval>(entity =>
            {
                entity.ToTable("scheduled_task_interval");

                entity.HasIndex(e => e.ScheduledTask)
                    .HasName("fk_SCHEDULED_TASK_INTERVAL_SCHEDULED_TASK1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Interval).HasColumnName("INTERVAL");

                entity.Property(e => e.ScheduledTask).HasColumnName("SCHEDULED_TASK");

                entity.HasOne(d => d.ScheduledTaskNavigation)
                    .WithMany(p => p.ScheduledTaskInterval)
                    .HasForeignKey(d => d.ScheduledTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SCHEDULED_TASK_INTERVAL_SCHEDULED_TASK1");
            });

            modelBuilder.Entity<ScheduledTaskLog>(entity =>
            {
                entity.ToTable("scheduled_task_log");

                entity.HasIndex(e => e.ScheduledTask)
                    .HasName("fk_TASK_AUCTION_SCAN_LOG_SCHEDULED_TASK1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Message)
                    .HasColumnName("MESSAGE")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ScheduledTask).HasColumnName("SCHEDULED_TASK");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.Tentatives).HasColumnName("TENTATIVES");

                entity.HasOne(d => d.ScheduledTaskNavigation)
                    .WithMany(p => p.ScheduledTaskLog)
                    .HasForeignKey(d => d.ScheduledTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TASK_AUCTION_SCAN_LOG_SCHEDULED_TASK1");
            });

            modelBuilder.Entity<ScheduledTaskType>(entity =>
            {
                entity.ToTable("scheduled_task_type");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Class)
                    .HasColumnName("CLASS")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
