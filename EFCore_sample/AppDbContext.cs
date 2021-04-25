using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    class AppDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Reward> Rewards { get; set; }

        // DB Connection string
        // set which DB connected (option, authorization)
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFCoreDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Model level filtering
            builder.Entity<Item>().HasQueryFilter(i => i.SoftDelete == false);

            // Create Index
            builder.Entity<Player>()
                .HasIndex(p => p.Name)
                .HasDatabaseName("Index_Person_Name")
                .IsUnique();

            // Build Relationship : one to many
            //builder.Entity<Player>()
            //    .HasMany(p => p.CreatedItems)
            //    .WithOne(i => i.Creator)
            //    .HasForeignKey(i => i.TestCreatorId);

            // Build Relationship : one to one
            builder.Entity<Player>()
                .HasOne(p => p.OwnedItem)
                .WithOne(i => i.Owner)
                .HasForeignKey<Item>(i => i.TestOwnerId);

            // Shadow Property
            builder.Entity<Item>().Property<DateTime>("RecoveredDate");

            // Backing Field Mapping
            //builder.Entity<Item>()
            //    .Property(i => i.JsonData)
            //    .HasField("_jsonData");

            // Owned Type
            builder.Entity<Item>()
                .OwnsOne(i => i.Option)
                .ToTable("ItemOption");

            // TPH Test
            builder.Entity<Item>()
                .HasDiscriminator(i => i.Type)
                .HasValue<Item>(ItemType.NormalItem)
                .HasValue<EventItem>(ItemType.EventItem);

            // Table Splitting
            builder.Entity<Item>()
                .HasOne(i => i.Detail)
                .WithOne()
                .HasForeignKey<ItemDetail>(i => i.ItemDetailId);

            builder.Entity<Item>().ToTable("Item");
            builder.Entity<ItemDetail>().ToTable("Item");

            // Backing Field + Relationship
            builder.Entity<Item>()
                .Metadata
                .FindNavigation("Reviews")
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            // UDF
            builder.HasDbFunction(() => Program.GetAverageReviewScore(0));

            // Default Value
            //builder.Entity<Item>()
            //    .Property("CreateDate")
            //    .HasDefaultValue(new DateTime(2021, 4, 21));

            // Default Value (SQL Segment)
            builder.Entity<Item>()
                .Property("CreateDate")
                .HasDefaultValueSql("GETDATE()");

            // Default Value (Value Generator)
            builder.Entity<Player>()
                .Property(p => p.Name)
                .HasValueGenerator((p, e) => new PlayerNameGenerator());

            /* Fluet API Sample
            
            builder.Entity<GameResult>()
              .ToTable("GameTableName");    // Set table name

            builder.Entity<GameResult>()
              .Property(g => g.RankingId)
              .HasColumnName("Ranking ID"); // set column name

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .IsUnicode(false);            // set Varchar. true == nVarchar

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .HasMaxLength(123);           // Set String Size

            builder.Entity<GameResult>()
              .Property(x => x.UserName)
              .IsRequired();                // set not null

            builder.Entity<GameResult>()
              .HasIndex(x => x.UserName);   // set index

            builder.Entity<GameResult>()
              .HasQueryFilter(p => p.SocreId > 1000); // set filter

            builder.Entity<GameResult>()
              .Ignore(g => g.Id);           // non mapping

            builder.Entity<GameResult>()
              .HasKey(x => new { x.RankingId, x.SocreId }); // Set PK (Composite key)

            builder.Entity<GameResult>()
              .HasIndex(x => new { x.RankingId, x.SocreId }); // Set index (Composit Index)

            builder.Entity<GameResult>()
              .HasIndex(g => g.RankingId)
              .HasName("ASD") // set Index name
              .IsUnique();	  // set UNIQUE Index

            */
        }
    }
}
